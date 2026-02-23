namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<int?> extractorRetries = new("--extractor-retries");
    private Option<bool> allowDynamicMpd = new("--allow-dynamic-mpd", "--no-ignore-dynamic-mpd");
    private Option<bool> ignoreDynamicMpd = new("--ignore-dynamic-mpd", "--no-allow-dynamic-mpd");
    private Option<bool> hlsSplitDiscontinuity = new("--hls-split-discontinuity");
    private Option<bool> noHlsSplitDiscontinuity = new("--no-hls-split-discontinuity");
    private MultiOption<string> extractorArgs = new("--extractor-args");

    public int? ExtractorRetries { get => extractorRetries.Value; set => extractorRetries.Value = value; }
    public bool AllowDynamicMpd { get => allowDynamicMpd.Value; set => allowDynamicMpd.Value = value; }
    public bool IgnoreDynamicMpd { get => ignoreDynamicMpd.Value; set => ignoreDynamicMpd.Value = value; }
    public bool HlsSplitDiscontinuity { get => hlsSplitDiscontinuity.Value; set => hlsSplitDiscontinuity.Value = value; }
    public bool NoHlsSplitDiscontinuity { get => noHlsSplitDiscontinuity.Value; set => noHlsSplitDiscontinuity.Value = value; }
    public MultiValue<string> ExtractorArgs { get => extractorArgs.Value; set => extractorArgs.Value = value; }
}
