namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<string> format = new("--format", "-f");
    private Option<string> formatSort = new("--format-sort", "-S");
    private Option<bool> formatSortForce = new("--format-sort-force");
    private Option<bool> noFormatSortForce = new("--no-format-sort-force");
    private Option<bool> videoMultistreams = new("--video-multistreams");
    private Option<bool> noVideoMultistreams = new("--no-video-multistreams");
    private Option<bool> audioMultistreams = new("--audio-multistreams");
    private Option<bool> noAudioMultistreams = new("--no-audio-multistreams");
    private Option<bool> preferFreeFormats = new("--prefer-free-formats");
    private Option<bool> noPreferFreeFormats = new("--no-prefer-free-formats");
    private Option<bool> checkFormats = new("--check-formats");
    private Option<bool> checkAllFormats = new("--check-all-formats");
    private Option<bool> noCheckFormats = new("--no-check-formats");
    private Option<bool> listFormats = new("--list-formats", "-F");
    private Option<DownloadMergeFormat> mergeOutputFormat = new("--merge-output-format");

    public string Format { get => format.Value; set => format.Value = value; }
    public string FormatSort { get => formatSort.Value; set => formatSort.Value = value; }
    public bool FormatSortForce { get => formatSortForce.Value; set => formatSortForce.Value = value; }
    public bool NoFormatSortForce { get => noFormatSortForce.Value; set => noFormatSortForce.Value = value; }
    public bool VideoMultistreams { get => videoMultistreams.Value; set => videoMultistreams.Value = value; }
    public bool NoVideoMultistreams { get => noVideoMultistreams.Value; set => noVideoMultistreams.Value = value; }
    public bool AudioMultistreams { get => audioMultistreams.Value; set => audioMultistreams.Value = value; }
    public bool NoAudioMultistreams { get => noAudioMultistreams.Value; set => noAudioMultistreams.Value = value; }
    public bool PreferFreeFormats { get => preferFreeFormats.Value; set => preferFreeFormats.Value = value; }
    public bool NoPreferFreeFormats { get => noPreferFreeFormats.Value; set => noPreferFreeFormats.Value = value; }
    public bool CheckFormats { get => checkFormats.Value; set => checkFormats.Value = value; }
    public bool CheckAllFormats { get => checkAllFormats.Value; set => checkAllFormats.Value = value; }
    public bool NoCheckFormats { get => noCheckFormats.Value; set => noCheckFormats.Value = value; }
    public bool ListFormats { get => listFormats.Value; set => listFormats.Value = value; }
    public DownloadMergeFormat MergeOutputFormat { get => mergeOutputFormat.Value; set => mergeOutputFormat.Value = value; }
}
