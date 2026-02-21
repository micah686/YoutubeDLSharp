using System.Text.RegularExpressions;
using YtdlpPocoGenerator.Models;

namespace YtdlpPocoGenerator.Generation;

/// <summary>
/// Infers C# types for yt-dlp fields purely from the field name, description,
/// inline type hints, and presence of sub-fields — no hardcoded field lists.
/// </summary>
public static class TypeMapper
{
    // Matches "Like 'subtitles'" or "Same as 'formats'" in a description.
    private static readonly Regex LikeReferenceRegex = new(
        @"(?:like|same as)\s+'([a-z_]+)'",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static string MapType(FieldDefinition field,
        IReadOnlyList<FieldDefinition>? allFields = null)
    {
        var name = field.Name;
        var desc = field.Description.ToLowerInvariant();

        // Special case: self-referential playlist entries.
        if (name == "entries")
            return "VideoData[]?";

        // Fields with parsed sub-fields → complex object types.
        if (field.SubFields.Count > 0)
        {
            var nestedClass = ToNestedClassName(name);

            // "dictionary in the format {tag: ...}" → Dictionary<string, T[]>?
            // This pattern is used by subtitles and automatic_captions.
            if (desc.Contains("dictionary") && (desc.Contains("in the format") || desc.Contains("{tag:") || desc.Contains("language")))
                return $"Dictionary<string, {nestedClass}[]>?";

            return $"{nestedClass}[]?";
        }

        // Resolve "Like 'subtitles'" / "Same as 'formats'" cross-references.
        // This handles automatic_captions and any future fields that defer to another field.
        if (allFields is not null)
        {
            var likeMatch = LikeReferenceRegex.Match(desc);
            if (likeMatch.Success)
            {
                var refName = likeMatch.Groups[1].Value;
                var refField = allFields.FirstOrDefault(f => f.Name == refName);
                if (refField is not null)
                    // Resolve without allFields to prevent infinite recursion.
                    return MapType(refField);
            }
        }

        // Description contains "dictionary" → generic dictionary fallback.
        if (desc.Contains("dictionary") || desc.Contains("in the format {"))
            return "Dictionary<string, object[]>?";

        // Explicit list-of-strings signals (categories, tags, cast, artists…)
        if (IsListOfStrings(desc))
            return "string[]?";

        // Use inline type hint from docstring annotation (e.g. the "int" from "(optional, int)")
        if (!string.IsNullOrEmpty(field.TypeHint))
        {
            var fromHint = MapTypeHint(field.TypeHint);
            if (fromHint is not null)
                return fromHint;
        }

        // Name-pattern rules — reliable for common suffixes.
        if (name.EndsWith("_count") || name.EndsWith("_number"))
            return "long?";
        if (name.EndsWith("_url"))
            return "string?";
        if (name.EndsWith("_timestamp"))
            return "long?";
        if (name.EndsWith("_date"))
            return "string?";
        if (name.EndsWith("_id"))
            return "string?";
        if (name.EndsWith("_key") || name.EndsWith("_note") || name.EndsWith("_type"))
            return "string?";
        if (name.EndsWith("_year"))
            return "int?";

        // Boolean-name prefixes.
        if (name.StartsWith("is_") || name.StartsWith("was_") ||
            name.StartsWith("has_") || name.StartsWith("no_"))
            return "bool?";

        // Description-based rules.
        if (desc.Contains("unix timestamp") || desc.Contains("posix timestamp"))
            return "long?";
        if (desc.StartsWith("whether") || desc.Contains("true/false") ||
            desc.Contains("true or false") || desc.StartsWith("boolean"))
            return "bool?";
        if (desc.Contains("integer") || desc.Contains("number of") ||
            desc.Contains("count of"))
            return "long?";
        if (desc.Contains("in seconds") || desc.Contains("length in seconds"))
            return "float?";
        if (desc.Contains("bitrate") || desc.Contains("kilobits per"))
            return "double?";
        if (desc.Contains("in pixels"))
            return "int?";

        return "string?";
    }

    /// <summary>
    /// Derives the C# nested class name from a plural field name.
    /// e.g. thumbnails → ThumbnailData, automatic_captions → AutomaticCaptionData
    /// </summary>
    public static string ToNestedClassName(string fieldName)
    {
        // Singularize: strip trailing 's' (thumbnails→thumbnail, formats→format…)
        // Special-case 'ies' ending (entries→entry) — but "entries" is handled before this.
        string singular;
        if (fieldName.EndsWith("ies"))
            singular = fieldName[..^3] + "y";
        else if (fieldName.EndsWith("s") && fieldName.Length > 2)
            singular = fieldName[..^1];
        else
            singular = fieldName;

        // snake_case → PascalCase
        var pascal = string.Concat(
            singular.Split('_')
                    .Select(p => p.Length > 0 ? char.ToUpperInvariant(p[0]) + p[1..] : ""));

        return pascal + "Data";
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private static bool IsListOfStrings(string desc) =>
        (desc.Contains("list of str") ||
         desc.Contains("list of tag") ||
         desc.Contains("list of categor") ||
         (desc.Contains("list of") && !desc.Contains("dict") && !desc.Contains("object"))) &&
        !desc.Contains("list of dict");

    private static string? MapTypeHint(string hint)
    {
        var h = hint.ToLowerInvariant().Trim();
        if (h == "int" || h == "integer") return "int?";
        if (h == "float") return "double?";
        if (h == "bool" || h == "boolean") return "bool?";
        if (h == "str" || h == "string") return "string?";
        return null;
    }
}
