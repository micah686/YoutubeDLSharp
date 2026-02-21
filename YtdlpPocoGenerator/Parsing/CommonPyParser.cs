using System.Text.RegularExpressions;
using YtdlpPocoGenerator.Models;

namespace YtdlpPocoGenerator.Parsing;

/// <summary>
/// Parses the InfoExtractor class docstring from yt-dlp's common.py to extract
/// field definitions for the info_dict. Fully driven by the docstring content —
/// no hardcoded field name lists.
/// </summary>
public static class CommonPyParser
{
    // Top-level field line: "    id:          Video identifier."
    // Groups: (1) leading spaces, (2) field name, (3) description start
    private static readonly Regex TopLevelFieldRegex = new(
        @"^( {4,8})([a-z][a-z0-9_]*):\s+(.+)$",
        RegexOptions.Compiled);

    // Sub-field bullet. Three separator styles are used in common.py:
    //   * url - description          (dash)
    //   * "url": description         (colon, used in subtitles section)
    //   * url        description     (two or more spaces, used in formats section)
    //   * width (optional, int) - …  (type annotation + dash)
    // Groups: (1) name, (2) type annotation, (3) description
    private static readonly Regex SubFieldBulletRegex = new(
        @"^\s+\*\s+[""']?([a-z][a-z0-9_]*)[""']?\s*(?:\(([^)]*)\))?\s*(?:[-–:]\s*|\s{2,})(.+)?$",
        RegexOptions.Compiled);

    // Lines that announce a new sub-section inside a field description.
    // These are skipped (not committed as field data) but don't end field collection.
    private static readonly Regex SubSectionAnnouncementRegex = new(
        @"^\s*(Potential fields|NOTE:|NB:|e\.g\.|DEPRECATED)",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    // Phrases that signal the field-definition block is about to start.
    private static readonly string[] FieldBlockTriggerPhrases =
    [
        "following field",
        "result of the _real_extract",
        "returns a dict",
        "returned result",
        "must be a dict",
    ];

    /// <summary>
    /// Parses common.py and returns all top-level info_dict field definitions.
    /// Fields that have nested bullet-point entries (thumbnails, formats, chapters…)
    /// will have their <see cref="FieldDefinition.SubFields"/> populated from those bullets.
    /// </summary>
    public static List<FieldDefinition> Parse(string source)
    {
        var lines = source.Split('\n');
        var (docStart, docEnd) = FindDocstring(lines);

        if (docStart < 0 || docEnd < 0)
            throw new InvalidOperationException("Could not find InfoExtractor class docstring.");

        return ParseAllFields(lines[docStart..docEnd]);
    }

    // ------------------------------------------------------------------
    // Docstring boundary detection
    // ------------------------------------------------------------------

    private static (int Start, int End) FindDocstring(string[] lines)
    {
        bool inInfoExtractor = false;
        int start = -1;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("class InfoExtractor"))
                inInfoExtractor = true;

            if (!inInfoExtractor) continue;

            if (lines[i].TrimStart().StartsWith("\"\"\""))
            {
                if (start < 0)
                    start = i;
                else
                    return (start, i);
            }
        }
        return (-1, -1);
    }

    // ------------------------------------------------------------------
    // Field parsing
    // ------------------------------------------------------------------

    private static List<FieldDefinition> ParseAllFields(string[] lines)
    {
        var fields = new List<FieldDefinition>();

        // Phase 1: wait for the trigger phrase that signals field defs are about to start.
        bool fieldBlockStarted = false;

        // Phase 2: once we see the first field line, lock in its indent level.
        int fieldIndent = -1;

        // Accumulator state for the current top-level field being parsed.
        FieldDefinition? currentField = null;
        var currentDesc = new List<string>();
        bool inSubFields = false;
        var currentSubFields = new List<FieldDefinition>();
        FieldDefinition? currentSubField = null;
        var currentSubDesc = new List<string>();

        void CommitSubField()
        {
            if (currentSubField is null) return;
            currentSubFields.Add(currentSubField with
            {
                Description = string.Join(" ", currentSubDesc).Trim()
            });
            currentSubField = null;
            currentSubDesc.Clear();
        }

        void CommitField()
        {
            if (currentField is null) return;
            CommitSubField();
            fields.Add(currentField with
            {
                Description = string.Join(" ", currentDesc).Trim(),
                SubFields = [.. currentSubFields]
            });
            currentField = null;
            currentDesc.Clear();
            currentSubFields.Clear();
            inSubFields = false;
        }

        foreach (var line in lines)
        {
            var trimmed = line.TrimEnd();
            var content = trimmed.TrimStart();

            // Phase 1 — scan for the trigger that says fields are coming.
            if (!fieldBlockStarted)
            {
                var lower = content.ToLowerInvariant();
                if (FieldBlockTriggerPhrases.Any(t => lower.Contains(t)))
                    fieldBlockStarted = true;
                continue;
            }

            // Check for a new top-level field definition.
            var fieldMatch = TopLevelFieldRegex.Match(trimmed);
            if (fieldMatch.Success)
            {
                int indent = fieldMatch.Groups[1].Length;

                // The first field line we see establishes the canonical indent level.
                if (fieldIndent < 0)
                    fieldIndent = indent;

                if (indent == fieldIndent)
                {
                    CommitField();
                    currentField = new FieldDefinition
                    {
                        Name = fieldMatch.Groups[2].Value,
                        IsOptional = false
                    };
                    currentDesc.Add(fieldMatch.Groups[3].Value.Trim());
                    inSubFields = false;
                    continue;
                }
            }

            if (currentField is null) continue;

            // Skip sub-section announcement lines — they're headers inside a field's
            // description block, not data to collect (e.g. "Potential fields:").
            if (SubSectionAnnouncementRegex.IsMatch(content))
                continue;

            // Sub-field bullet point.
            if (content.StartsWith("*"))
            {
                var subMatch = SubFieldBulletRegex.Match(trimmed);
                if (subMatch.Success)
                {
                    inSubFields = true;
                    CommitSubField();
                    currentSubField = new FieldDefinition
                    {
                        Name = subMatch.Groups[1].Value,
                        TypeHint = ExtractBaseTypeHint(subMatch.Groups[2].Value),
                        Description = subMatch.Groups[3].Value.Trim(),
                        IsOptional = subMatch.Groups[2].Value
                            .Contains("optional", StringComparison.OrdinalIgnoreCase)
                    };
                    currentSubDesc.Add(subMatch.Groups[3].Value.Trim());
                    continue;
                }
            }

            // Continuation line — appended to whichever scope is active.
            if (!string.IsNullOrWhiteSpace(content))
            {
                if (inSubFields && currentSubField is not null)
                    currentSubDesc.Add(content);
                else
                    currentDesc.Add(content);
            }
        }

        CommitField();
        return fields;
    }

    /// <summary>
    /// From a raw annotation string like "optional, int" extracts just the base
    /// type token ("int"), stripping "optional" and similar qualifiers.
    /// </summary>
    private static string ExtractBaseTypeHint(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

        // Strip "optional" and any surrounding spaces/commas.
        var cleaned = Regex.Replace(raw, @"\boptional\b", "", RegexOptions.IgnoreCase)
                           .Trim(' ', ',');

        return cleaned.Trim();
    }
}
