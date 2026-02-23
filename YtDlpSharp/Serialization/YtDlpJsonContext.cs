using YtdlpPoco;

namespace YtDlpSharp.Serialization;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(VideoData))]
[JsonSerializable(typeof(FormatData))]
[JsonSerializable(typeof(ThumbnailData))]
[JsonSerializable(typeof(SubtitleData))]
[JsonSerializable(typeof(CommentData))]
[JsonSerializable(typeof(ChapterData))]
[JsonSerializable(typeof(HeatmapData))]
[JsonSerializable(typeof(VideoData[]))]
[JsonSerializable(typeof(Dictionary<string, SubtitleData[]>))]
public partial class YtDlpJsonContext : JsonSerializerContext;
