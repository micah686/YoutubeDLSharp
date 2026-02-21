using System.Text.RegularExpressions;
using YtdlpPocoGenerator.Models;

namespace YtdlpPocoGenerator.Parsing;

/// <summary>
/// Parses the InfoExtractor class docstring from yt-dlp's common.py
/// to extract field definitions for the info_dict.
/// </summary>
public static class CommonPyParser
{
    // Matches lines like:    id:             Video identifier.
    // Group 1 = field name, Group 2 = description start
    private static readonly Regex FieldLineRegex = new(
        @"^( {4,8})([a-z][a-z0-9_]*):\s+(.+)$",
        RegexOptions.Compiled);

    // Matches sub-field bullet lines like:  * "url"  or  * url (optional, int) - description
    private static readonly Regex SubFieldRegex = new(
        @"^\s+\*\s+[""']?([a-z][a-z0-9_]*)[""']?\s*(?:\(([^)]*)\))?\s*(?:[-–]\s*(.*))?$",
        RegexOptions.Compiled);

    private static readonly Regex SectionHeaderRegex = new(
        @"^\s*(Additionally|The following|Required|Optional|Each|Deprecated)",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static (List<FieldDefinition> VideoFields, List<FieldDefinition> FormatFields) Parse(string source)
    {
        var lines = source.Split('\n');
        var docstringStart = FindDocstringStart(lines);
        var docstringEnd = FindDocstringEnd(lines, docstringStart);

        if (docstringStart < 0 || docstringEnd < 0)
            throw new InvalidOperationException("Could not find InfoExtractor class docstring.");

        var docLines = lines[docstringStart..docstringEnd];

        var videoFields = ParseFieldBlock(docLines, "format_dict", stopAt: "format_dict");
        var formatFields = ParseFormatFields(docLines);

        return (videoFields, formatFields);
    }

    private static int FindDocstringStart(string[] lines)
    {
        bool inInfoExtractor = false;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("class InfoExtractor"))
                inInfoExtractor = true;

            if (inInfoExtractor && lines[i].TrimStart().StartsWith("\"\"\""))
                return i;
        }
        return -1;
    }

    private static int FindDocstringEnd(string[] lines, int start)
    {
        if (start < 0) return -1;
        for (int i = start + 1; i < lines.Length; i++)
        {
            var trimmed = lines[i].TrimStart();
            if (trimmed.StartsWith("\"\"\""))
                return i;
        }
        return -1;
    }

    private static List<FieldDefinition> ParseFieldBlock(string[] lines, string startMarker, string stopAt)
    {
        var fields = new List<FieldDefinition>();
        bool inFieldBlock = false;
        int fieldIndent = -1;

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

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var trimmed = line.TrimEnd();

            // Detect stop marker (format_dict section begins)
            if (stopAt == "format_dict" && trimmed.Contains("format_dict:") && trimmed.Contains("Potential fields"))
            {
                CommitField();
                break;
            }

            // Check for field line
            var fieldMatch = FieldLineRegex.Match(trimmed);
            if (fieldMatch.Success)
            {
                int indent = fieldMatch.Groups[1].Length;

                // Start collecting once we see the first field-like line in the right indent range
                if (!inFieldBlock && IsInfoDictField(fieldMatch.Groups[2].Value))
                {
                    inFieldBlock = true;
                    fieldIndent = indent;
                }

                if (inFieldBlock && indent == fieldIndent)
                {
                    CommitField();
                    currentField = new FieldDefinition
                    {
                        Name = fieldMatch.Groups[2].Value,
                        IsOptional = false // will determine from section context
                    };
                    currentDesc.Add(fieldMatch.Groups[3].Value.Trim());
                    inSubFields = false;
                    continue;
                }
            }

            if (!inFieldBlock || currentField is null) continue;

            // Check for sub-field bullet
            var subMatch = SubFieldRegex.Match(trimmed);
            if (subMatch.Success && trimmed.TrimStart().StartsWith("*"))
            {
                inSubFields = true;
                CommitSubField();
                currentSubField = new FieldDefinition
                {
                    Name = subMatch.Groups[1].Value,
                    Description = subMatch.Groups[3].Value.Trim(),
                    IsOptional = subMatch.Groups[2].Value.Contains("optional")
                };
                currentSubDesc.Add(subMatch.Groups[3].Value.Trim());
                continue;
            }

            // Section headers reset context
            if (SectionHeaderRegex.IsMatch(trimmed))
            {
                CommitField();
                continue;
            }

            // Continuation line for description
            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                if (inSubFields && currentSubField is not null)
                    currentSubDesc.Add(trimmed.TrimStart());
                else
                    currentDesc.Add(trimmed.TrimStart());
            }
        }

        CommitField();
        return fields;
    }

    private static List<FieldDefinition> ParseFormatFields(string[] lines)
    {
        var fields = new List<FieldDefinition>();
        bool inFormatSection = false;
        bool inPotentialFields = false;

        FieldDefinition? currentField = null;
        var currentDesc = new List<string>();

        void CommitField()
        {
            if (currentField is null) return;
            fields.Add(currentField with { Description = string.Join(" ", currentDesc).Trim() });
            currentField = null;
            currentDesc.Clear();
        }

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].TrimEnd();

            if (!inFormatSection && (line.Contains("format_dict") || line.Contains("formats:")))
                inFormatSection = true;

            if (!inFormatSection) continue;

            if (line.Contains("Potential fields") || line.Contains("The following fields"))
            {
                inPotentialFields = true;
                continue;
            }

            if (!inPotentialFields) continue;

            // End of format section
            if (line.TrimStart().StartsWith("subtitles:") || line.Contains("The following fields are extracted"))
            {
                CommitField();
                break;
            }

            var subMatch = SubFieldRegex.Match(line);
            if (subMatch.Success && line.TrimStart().StartsWith("*"))
            {
                CommitField();
                currentField = new FieldDefinition
                {
                    Name = subMatch.Groups[1].Value,
                    Description = subMatch.Groups[3].Value.Trim(),
                    IsOptional = subMatch.Groups[2].Value.Contains("optional")
                };
                currentDesc.Add(subMatch.Groups[3].Value.Trim());
                continue;
            }

            if (currentField is not null && !string.IsNullOrWhiteSpace(line))
                currentDesc.Add(line.TrimStart());
        }

        CommitField();
        return fields;
    }

    // Known top-level info_dict field names to seed detection
    private static readonly HashSet<string> KnownInfoDictFields =
    [
        "id", "title", "formats", "url", "ext", "description", "thumbnail",
        "thumbnails", "uploader", "upload_date", "timestamp", "duration",
        "view_count", "like_count", "comment_count", "age_limit", "webpage_url",
        "categories", "tags", "is_live", "was_live", "live_status", "chapters",
        "subtitles", "automatic_captions", "series", "season", "episode",
        "track", "album", "artist", "genre", "alt_title", "display_id",
        "format_id", "format", "format_note", "player_url", "direct"
    ];

    private static bool IsInfoDictField(string name) =>
        KnownInfoDictFields.Contains(name) || name.EndsWith("_count") ||
        name.EndsWith("_id") || name.EndsWith("_url") || name.EndsWith("_date") ||
        name.EndsWith("_number") || name.EndsWith("_timestamp");
}
