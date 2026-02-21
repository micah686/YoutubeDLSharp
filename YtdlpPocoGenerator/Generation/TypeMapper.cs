namespace YtdlpPocoGenerator.Generation;

/// <summary>
/// Maps yt-dlp Python field names and descriptions to C# types.
/// </summary>
public static class TypeMapper
{
    // Explicit overrides for fields with non-obvious types
    private static readonly Dictionary<string, string> ExplicitTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        // VideoData fields
        ["id"] = "string?",
        ["title"] = "string?",
        ["alt_title"] = "string?",
        ["display_id"] = "string?",
        ["url"] = "string?",
        ["ext"] = "string?",
        ["format"] = "string?",
        ["format_id"] = "string?",
        ["format_note"] = "string?",
        ["player_url"] = "string?",
        ["description"] = "string?",
        ["thumbnail"] = "string?",
        ["thumbnails"] = "ThumbnailData[]?",
        ["formats"] = "FormatData[]?",
        ["subtitles"] = "Dictionary<string, SubtitleData[]>?",
        ["automatic_captions"] = "Dictionary<string, SubtitleData[]>?",
        ["chapters"] = "ChapterData[]?",
        ["comments"] = "CommentData[]?",
        ["entries"] = "VideoData[]?",
        ["categories"] = "string[]?",
        ["tags"] = "string[]?",
        ["cast"] = "string[]?",
        ["artists"] = "string[]?",
        ["genres"] = "string[]?",
        ["composers"] = "string[]?",
        ["album_artists"] = "string[]?",
        ["uploader"] = "string?",
        ["uploader_id"] = "string?",
        ["uploader_url"] = "string?",
        ["channel"] = "string?",
        ["channel_id"] = "string?",
        ["channel_url"] = "string?",
        ["channel_follower_count"] = "long?",
        ["channel_is_verified"] = "bool?",
        ["location"] = "string?",
        ["license"] = "string?",
        ["creator"] = "string?",
        ["duration"] = "float?",
        ["timestamp"] = "long?",
        ["release_timestamp"] = "long?",
        ["modified_timestamp"] = "long?",
        ["upload_date"] = "string?",
        ["release_date"] = "string?",
        ["modified_date"] = "string?",
        ["view_count"] = "long?",
        ["concurrent_view_count"] = "long?",
        ["like_count"] = "long?",
        ["dislike_count"] = "long?",
        ["repost_count"] = "long?",
        ["comment_count"] = "long?",
        ["average_rating"] = "double?",
        ["age_limit"] = "int?",
        ["webpage_url"] = "string?",
        ["webpage_url_basename"] = "string?",
        ["webpage_url_domain"] = "string?",
        ["is_live"] = "bool?",
        ["was_live"] = "bool?",
        ["live_status"] = "string?",
        ["start_time"] = "float?",
        ["end_time"] = "float?",
        ["playable_in_embed"] = "string?",
        ["availability"] = "string?",
        ["direct"] = "bool?",
        ["chapter"] = "string?",
        ["chapter_number"] = "int?",
        ["chapter_id"] = "string?",
        ["chapter_start_time"] = "float?",
        ["chapter_end_time"] = "float?",
        ["series"] = "string?",
        ["series_id"] = "string?",
        ["season"] = "string?",
        ["season_number"] = "int?",
        ["season_id"] = "string?",
        ["episode"] = "string?",
        ["episode_number"] = "int?",
        ["episode_id"] = "string?",
        ["track"] = "string?",
        ["track_number"] = "int?",
        ["track_id"] = "string?",
        ["artist"] = "string?",
        ["genre"] = "string?",
        ["album"] = "string?",
        ["album_type"] = "string?",
        ["album_artist"] = "string?",
        ["disc_number"] = "int?",
        ["release_year"] = "int?",
        ["composer"] = "string?",
        ["section_start"] = "long?",
        ["section_end"] = "long?",
        ["rows"] = "long?",
        ["columns"] = "long?",
        ["extractor"] = "string?",
        ["extractor_key"] = "string?",

        // FormatData fields
        ["manifest_url"] = "string?",
        ["width"] = "int?",
        ["height"] = "int?",
        ["resolution"] = "string?",
        ["dynamic_range"] = "string?",
        ["tbr"] = "double?",
        ["abr"] = "double?",
        ["acodec"] = "string?",
        ["asr"] = "double?",
        ["audio_channels"] = "int?",
        ["vbr"] = "double?",
        ["fps"] = "float?",
        ["vcodec"] = "string?",
        ["container"] = "string?",
        ["filesize"] = "long?",
        ["filesize_approx"] = "long?",
        ["protocol"] = "string?",
        ["fragment_base_url"] = "string?",
        ["is_from_start"] = "bool?",
        ["preference"] = "int?",
        ["language"] = "string?",
        ["language_preference"] = "int?",
        ["quality"] = "double?",
        ["source_preference"] = "int?",
        ["stretched_ratio"] = "float?",
        ["no_resume"] = "bool?",
        ["has_drm"] = "bool?",
    };

    public static string MapType(string fieldName, string description)
    {
        if (ExplicitTypes.TryGetValue(fieldName, out var explicit_type))
            return explicit_type;

        return InferFromNameAndDescription(fieldName, description);
    }

    private static string InferFromNameAndDescription(string name, string desc)
    {
        var descLower = desc.ToLowerInvariant();

        // Name-based suffix rules
        if (name.EndsWith("_count") || name.EndsWith("_number"))
            return "long?";
        if (name.EndsWith("_url") || name.EndsWith("_key") || name.EndsWith("_note"))
            return "string?";
        if (name.EndsWith("_timestamp"))
            return "long?";
        if (name.EndsWith("_date"))
            return "string?";
        if (name.EndsWith("_id"))
            return "string?";

        // Description-based rules
        if (descLower.Contains("list of") || descLower.Contains("a list"))
            return "string[]?";
        if (descLower.Contains("dictionary") || descLower.Contains("dict of"))
            return "Dictionary<string, object>?";
        if (descLower.StartsWith("boolean") || descLower.StartsWith("whether") ||
            descLower.Contains("true/false") || descLower.Contains("true or false"))
            return "bool?";
        if (descLower.Contains("integer") || descLower.Contains("number of") ||
            descLower.Contains("count of"))
            return "long?";
        if (descLower.Contains("float") || descLower.Contains("seconds") ||
            descLower.Contains("bitrate") || descLower.Contains("fps"))
            return "double?";

        return "string?";
    }
}
