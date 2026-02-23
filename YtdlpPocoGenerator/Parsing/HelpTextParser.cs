using System.Text.RegularExpressions;
using YtdlpPocoGenerator.Models;

namespace YtdlpPocoGenerator.Parsing;

/// <summary>
/// Parses yt-dlp's help text (from --help output or the README) into
/// <see cref="OptionSection"/>s containing <see cref="OptionDefinition"/>s.
/// This is the C# equivalent of autoconvert.py's parsing logic.
/// </summary>
public static class HelpTextParser
{
    // Column where descriptions start in yt-dlp help output.
    private const int DescriptionIndent = 36;

    // Column where descriptions start in deprecated options text.
    private const int DeprecatedDescriptionIndent = 33;

    // Option lines start with 4 spaces then a dash.
    private const int OptionIndent = 4;

    /// <summary>
    /// Maps certain option names to specific C# types (matching Python TYPE_MAP).
    /// </summary>
    private static readonly Dictionary<string, string> TypeMap = new(StringComparer.Ordinal)
    {
        ["LimitRate"] = "long?",
        ["ThrottledRate"] = "long?",
        ["Retries"] = "int?",
        ["FileAccessRetries"] = "int?",
        ["FragmentRetries"] = "int?",
        ["BufferSize"] = "long?",
        ["HttpChunkSize"] = "long?",
        ["AutonumberStart"] = "int?",
        ["SocketTimeout"] = "int?",
        ["AudioFormat"] = "AudioConversionFormat",
        ["AudioQuality"] = "byte?",
        ["RecodeVideo"] = "VideoRecodeFormat",
        ["MergeOutputFormat"] = "DownloadMergeFormat",
        ["PlaylistStart"] = "int?",
        ["PlaylistEnd"] = "int?",
        ["MaxDownloads"] = "int?",
        ["Date"] = "DateTime",
        ["DateBefore"] = "DateTime",
        ["DateAfter"] = "DateTime",
        ["MinViews"] = "long?",
        ["MaxViews"] = "long?",
        ["AgeLimit"] = "byte?",
        ["SleepRequests"] = "int?",
        ["SleepInterval"] = "int?",
        ["MaxSleepInterval"] = "int?",
        ["SleepSubtitles"] = "int?",
        ["SkipPlaylistAfterErrors"] = "int?",
        ["ConcurrentFragments"] = "int?",
        ["TrimFilenames"] = "int?",
        ["ExtractorRetries"] = "int?",
    };

    /// <summary>
    /// Maps option names that need translation to correct PascalCase (matching Python TRANSLATE_MAP).
    /// </summary>
    private static readonly Dictionary<string, string> TranslateMap = new(StringComparer.Ordinal)
    {
        ["Twofactor"] = "TwoFactor",
        ["RmCacheDir"] = "RemoveCacheDir",
        ["ForceIpv4"] = "ForceIPv4",
        ["ForceIpv6"] = "ForceIPv6",
        ["Datebefore"] = "DateBefore",
        ["Dateafter"] = "DateAfter",
    };

    /// <summary>
    /// Options that are always treated as multi-options regardless of description.
    /// </summary>
    private static readonly HashSet<string> MultiOptions = ["Paths"];

    /// <summary>
    /// Regex to detect (Alias: --flag) patterns in descriptions.
    /// </summary>
    private static readonly Regex AliasRegex = new(
        @"\(Alias\:\s+(\S+)\)",
        RegexOptions.Compiled);

    /// <summary>
    /// Regex to split the README on markdown ## section headers.
    /// Captures "## Section Name:" headers to separate option groups.
    /// </summary>
    private static readonly Regex SectionHeaderRegex = new(
        @"^##\s+(.+?):\s*$",
        RegexOptions.Compiled | RegexOptions.Multiline);

    /// <summary>
    /// Parses yt-dlp help text (from the README markdown) into sections of option definitions.
    /// The README uses "## Section Name:" markdown headers to separate sections.
    /// </summary>
    public static List<OptionSection> Parse(string helpText)
    {
        // Forward-compatibility: strip Adobe Pass Options section header
        helpText = Regex.Replace(helpText, @"\n\s+Adobe Pass Options:", "");

        // Find all section headers and split the text between them
        var headerMatches = SectionHeaderRegex.Matches(helpText);
        var result = new List<OptionSection>();

        for (int i = 0; i < headerMatches.Count; i++)
        {
            var headerMatch = headerMatches[i];
            var rawSectionName = headerMatch.Groups[1].Value.Trim();

            // Determine the section body (text between this header and the next)
            var bodyStart = headerMatch.Index + headerMatch.Length;
            var bodyEnd = (i + 1 < headerMatches.Count)
                ? headerMatches[i + 1].Index
                : helpText.Length;
            var body = helpText[bodyStart..bodyEnd];

            // Derive the section name
            var sectionName = PrepareName(rawSectionName
                .Replace("Options", "")
                .Trim());

            if (sectionName == "PresetAliases" || string.IsNullOrWhiteSpace(sectionName))
                continue;

            // Extract option lines (lines starting with spaces)
            var lines = body.Split('\n')
                .Where(line => line.StartsWith("    "))
                .ToArray();

            if (lines.Length == 0)
                continue;

            var options = ExtractOptions(lines);
            Console.WriteLine($"  {sectionName}: Found {options.Count} options.");

            result.Add(new OptionSection
            {
                Name = sectionName,
                Options = options
            });
        }

        return result;
    }

    /// <summary>
    /// Parses deprecated options from the README's deprecated options section.
    /// The format is a fixed-width layout where the replacement starts at column 33.
    /// </summary>
    public static List<OptionDefinition> ParseDeprecatedOptions(string text)
    {
        var items = new List<OptionDefinition>();

        foreach (var line in text.Split('\n'))
        {
            // Deprecated option lines start with spaces and a dash
            if (!line.TrimStart().StartsWith("-"))
                continue;

            // Must be long enough to have a description
            if (line.Length <= DeprecatedDescriptionIndent)
                continue;

            var optionPart = line[..DeprecatedDescriptionIndent];
            var descr = line[DeprecatedDescriptionIndent..].Trim();

            // Skip lines where description starts with "Default"
            if (descr.StartsWith("Default", StringComparison.Ordinal))
                continue;

            var literals = optionPart.Split(',')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Split(' ')[0])
                .ToList();

            if (literals.Count == 0)
                continue;

            var name = PrepareName(literals[^1]);
            var deprecatedDescr = $"Deprecated in favor of: {descr}.";
            var csharpType = InferType(name, optionPart.Trim());

            items.Add(new OptionDefinition
            {
                Name = name,
                CSharpType = csharpType,
                Literals = literals,
                DescriptionLines = [deprecatedDescr],
                IsMulti = false,
                IsDeprecated = true,
            });
        }

        Console.WriteLine($"  Deprecated: Found {items.Count} options.");
        return items;
    }

    /// <summary>
    /// Extracts individual option definitions from lines within a section.
    /// </summary>
    private static List<OptionDefinition> ExtractOptions(string[] lines)
    {
        var items = new List<(List<string> Literals, List<string> Description)>();
        List<string>? currentLiterals = null;
        List<string>? currentDescr = null;

        foreach (var line in lines)
        {
            // Continuation line (indented to description column)
            if (line.Length >= DescriptionIndent &&
                line[..(DescriptionIndent)].All(c => c == ' ') &&
                line.Length > DescriptionIndent)
            {
                currentDescr?.Add(line[DescriptionIndent..]);
            }
            // New option line (starts with spaces + dash)
            else if (line.Length >= OptionIndent &&
                     line[..OptionIndent] == "    " &&
                     line[OptionIndent] == '-')
            {
                // Commit previous item
                if (currentLiterals != null && currentDescr != null)
                {
                    items.Add((currentLiterals, currentDescr));
                }

                // Check if description starts on the same line
                if (line.Length > DescriptionIndent && line[DescriptionIndent - 1] == ' ')
                {
                    currentLiterals = line[..DescriptionIndent]
                        .Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToList();
                    currentDescr = [line[DescriptionIndent..]];
                }
                else
                {
                    currentLiterals = line.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToList();
                    currentDescr = [];
                }
            }
        }

        // Commit last item
        if (currentLiterals != null && currentDescr != null)
        {
            items.Add((currentLiterals, currentDescr));
        }

        // Convert to OptionDefinitions
        var result = new List<OptionDefinition>();
        foreach (var (literals, description) in items)
        {
            // Extract the flag names (first token of each literal)
            var flags = literals.Select(s => s.Split(' ')[0]).ToList();
            var name = PrepareName(flags[^1]);
            var csharpType = InferType(name, literals[^1]);

            // Check if multi-option
            var joinedDescr = string.Join(" ", description);
            var isMulti = joinedDescr.Contains("multiple times", StringComparison.OrdinalIgnoreCase) ||
                          MultiOptions.Contains(name);

            // Extract alias options from description
            var aliasMatch = AliasRegex.Match(joinedDescr);
            if (aliasMatch.Success)
            {
                flags.Add(aliasMatch.Groups[1].Value);
            }

            result.Add(new OptionDefinition
            {
                Name = name,
                CSharpType = csharpType,
                Literals = flags,
                DescriptionLines = description,
                IsMulti = isMulti,
                IsDeprecated = false,
            });
        }

        return result;
    }

    /// <summary>
    /// Converts a CLI flag name like "--limit-rate" to PascalCase "LimitRate".
    /// Equivalent to Python's prepare_name().
    /// </summary>
    internal static string PrepareName(string flag)
    {
        // Strip leading dashes
        var name = flag.TrimStart('-');

        // Convert "kebab-case/slash" to title case (matches Python str.title() behavior:
        // uppercase first letter, lowercase the rest of each word)
        name = string.Concat(
            name.Replace("-", " ")
                .Replace("/", " and ")
                .Split(' ')
                .Select(w => w.Length == 0 ? "" : char.ToUpperInvariant(w[0]) + w[1..].ToLowerInvariant())
        );

        // Apply translation map
        if (TranslateMap.TryGetValue(name, out var translated))
            name = translated;

        return name;
    }

    /// <summary>
    /// Infers the C# type from the option name and its raw text.
    /// Equivalent to Python's infer_type().
    /// </summary>
    private static string InferType(string name, string rawText)
    {
        var parts = rawText.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 1)
            return "bool";

        if (TypeMap.TryGetValue(name, out var mappedType))
            return mappedType;

        if (parts.Length > 2)
            return "OptionVals";

        return "string";
    }
}
