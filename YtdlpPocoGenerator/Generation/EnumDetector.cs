using System.Text.RegularExpressions;
using YtdlpPocoGenerator.Models;

namespace YtdlpPocoGenerator.Generation;

/// <summary>
/// Scans field descriptions for closed sets of quoted string values and produces
/// <see cref="EnumDefinition"/> objects. Detection is fully driven by docstring text.
/// </summary>
public static class EnumDetector
{
    // Single-quoted identifiers: 'is_live', 'premium_only', 'SDR'
    private static readonly Regex SingleQuotedRegex = new(
        @"'([A-Za-z][A-Za-z0-9_+\-.]*)'",
        RegexOptions.Compiled);

    // Double-quoted identifiers: "SDR", "HDR10+", "HLG"
    private static readonly Regex DoubleQuotedRegex = new(
        @"""([A-Za-z][A-Za-z0-9_+\-.]*)""",
        RegexOptions.Compiled);

    // Explicit closed-set signals: "One of …", "Can be …", "Following values …"
    private static readonly Regex ExplicitSignalRegex = new(
        @"\bone\s+of\b|\bcan\s+be\b|\bfollowing\s+values\b",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    // Implicit signal: three or more comma-separated quoted values in a row.
    // e.g.  'is_live', 'is_upcoming', 'was_live'
    private static readonly Regex CommaListSignalRegex = new(
        @"(?:'[^']+'|""[^""]+""),\s*(?:'[^']+'|""[^""]+""),\s*(?:'[^']+'|""[^""]+"")",
        RegexOptions.Compiled);

    // Values to never include in an enum.
    private static readonly HashSet<string> GlobalExclusions =
        new(StringComparer.OrdinalIgnoreCase)
        { "None", "True", "False", "null", "true", "false" };

    private const int MaxValueLength = 30;

    /// <summary>
    /// Detects enums in top-level fields and their sub-fields.
    /// Returns a dict keyed by field name.
    /// </summary>
    public static Dictionary<string, EnumDefinition> Detect(List<FieldDefinition> topLevelFields)
    {
        var result = new Dictionary<string, EnumDefinition>(StringComparer.OrdinalIgnoreCase);

        foreach (var field in topLevelFields)
        {
            TryAdd(result, field);

            foreach (var sub in field.SubFields)
                TryAdd(result, sub);
        }

        return result;
    }

    private static void TryAdd(Dictionary<string, EnumDefinition> result, FieldDefinition field)
    {
        var def = TryDetect(field);
        if (def is not null)
            result.TryAdd(field.Name, def);
    }

    // "e.g." immediately before the comma-list means examples, not a closed set.
    private static readonly Regex EgSignalRegex = new(
        @"\be\.g\.?\s+['""]",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static EnumDefinition? TryDetect(FieldDefinition field)
    {
        // Fields with sub-fields are complex object types — never enums.
        if (field.SubFields.Count > 0) return null;

        var desc = field.Description;
        if (string.IsNullOrWhiteSpace(desc)) return null;

        // Deprecated redirect fields: "Use 'X' instead." — not an enum.
        var trimmed = desc.TrimStart();
        if (trimmed.StartsWith("Use ", StringComparison.OrdinalIgnoreCase) ||
            trimmed.StartsWith("DEPRECATED", StringComparison.OrdinalIgnoreCase))
            return null;

        // Fields whose name implies a count are always numeric, never string enums.
        if (field.Name.EndsWith("_count") || field.Name.EndsWith("_number"))
            return null;

        bool hasExplicitSignal = ExplicitSignalRegex.IsMatch(desc);
        bool hasCommaList = CommaListSignalRegex.IsMatch(desc);

        // Require at least one strong signal to avoid false positives.
        if (!hasExplicitSignal && !hasCommaList) return null;

        // "e.g. 'episode', 'clip'" means examples, not an exhaustive set.
        // Only reject this if there's no explicit "one of" overriding the e.g.
        if (!hasExplicitSignal && EgSignalRegex.IsMatch(desc)) return null;

        // Extract values from the specific span where the enum values appear,
        // rather than the entire description, to avoid picking up cross-references.
        var values = ExtractValuesFromEnumSpan(desc, hasExplicitSignal);

        // With explicit signal require 2+; with only comma-list signal require 3+.
        int minValues = hasExplicitSignal ? 2 : 3;
        if (values.Count < minValues) return null;

        return new EnumDefinition
        {
            Name = ToPascalCase(field.Name),
            FieldName = field.Name,
            Summary = FirstSentence(desc),
            Values = values.Select(v => (v, ToMemberName(v))).ToList()
        };
    }

    /// <summary>
    /// Extracts enum values only from the relevant span of the description:
    /// — for explicit signals ("one of …"): from that phrase to the sentence end
    /// — for comma-list signal: the largest run of consecutive comma-separated
    ///   quoted values
    /// </summary>
    private static List<string> ExtractValuesFromEnumSpan(string desc, bool hasExplicitSignal)
    {
        if (hasExplicitSignal)
        {
            // Find the "one of" phrase and take just that sentence.
            var signalMatch = ExplicitSignalRegex.Match(desc);
            if (signalMatch.Success)
            {
                var spanStart = signalMatch.Index;
                var sentenceEnd = desc.IndexOf('.', spanStart);
                var span = sentenceEnd > spanStart
                    ? desc[spanStart..sentenceEnd]
                    : desc[spanStart..];

                var values = ExtractValues(SingleQuotedRegex, span);
                if (values.Count == 0)
                    values = ExtractValues(DoubleQuotedRegex, span);
                return values;
            }
        }

        // Comma-list signal: extract the longest consecutive run of quoted values.
        // This prevents picking up stray quoted values elsewhere in the description.
        return ExtractLongestCommaList(desc);
    }

    /// <summary>
    /// Finds the longest run of comma-separated quoted values in <paramref name="desc"/>
    /// and returns just those values.
    /// </summary>
    private static List<string> ExtractLongestCommaList(string desc)
    {
        // Match all quoted tokens with optional trailing commas.
        var tokenRegex = new Regex(
            @"(?:'([A-Za-z][A-Za-z0-9_+\-.]*)'|""([A-Za-z][A-Za-z0-9_+\-.]*)""),?\s*");

        var runs = new List<List<string>>();
        var current = new List<string>();
        int lastEnd = -1;

        foreach (Match m in tokenRegex.Matches(desc))
        {
            var val = m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value;
            if (string.IsNullOrEmpty(val)) continue;
            if (GlobalExclusions.Contains(val) || val.Contains('.') || val.Length > MaxValueLength)
                continue;

            // Consider it "consecutive" if it starts within a few chars of the last token's end.
            bool consecutive = lastEnd >= 0 && m.Index - lastEnd <= 5;
            if (consecutive)
            {
                current.Add(val);
            }
            else
            {
                if (current.Count > 0) runs.Add(current);
                current = [val];
            }
            lastEnd = m.Index + m.Length;
        }
        if (current.Count > 0) runs.Add(current);

        return runs.Count == 0 ? [] :
               runs.OrderByDescending(r => r.Count).First()
                   .Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    private static List<string> ExtractValues(Regex regex, string desc)
    {
        return regex.Matches(desc)
                    .Select(m => m.Groups[1].Value)
                    .Where(v =>
                        v.Length <= MaxValueLength &&
                        !GlobalExclusions.Contains(v) &&
                        !v.Contains('.'))   // exclude method/class refs like InfoExtractor._foo
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
    }

    // ------------------------------------------------------------------
    // Name utilities
    // ------------------------------------------------------------------

    private static string ToPascalCase(string snake) =>
        string.Concat(
            snake.Split('_')
                 .Select(p => p.Length > 0 ? char.ToUpperInvariant(p[0]) + p[1..] : ""));

    /// <summary>
    /// Converts a raw enum value string to a valid C# member name.
    /// "is_live" → "IsLive", "HDR10+" → "Hdr10Plus", "Full-length" → "FullLength"
    /// </summary>
    internal static string ToMemberName(string value)
    {
        // Treat hyphens as word separators (Full-length → Full_length → FullLength).
        // Substitute other special characters.
        var sanitized = value
            .Replace("-", "_")
            .Replace("+", "Plus")
            .Replace(".", "Dot");

        var result = string.Concat(
            sanitized.Split('_')
                     .Select(p => p.Length == 0 ? "" : char.ToUpperInvariant(p[0]) + p[1..]));

        // Ensure the name starts with a letter (defensive).
        if (result.Length > 0 && !char.IsLetter(result[0]))
            result = "V" + result;

        return result;
    }

    private static string FirstSentence(string text)
    {
        var idx = text.IndexOf('.');
        return (idx > 0 ? text[..(idx + 1)] : text).Trim();
    }
}
