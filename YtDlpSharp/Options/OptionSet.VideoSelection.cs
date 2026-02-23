namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<string> playlistItems = new("-I", "--playlist-items");
    private Option<string> minFilesize = new("--min-filesize");
    private Option<string> maxFilesize = new("--max-filesize");
    private Option<DateTime> date = new("--date");
    private Option<DateTime> dateBefore = new("--datebefore");
    private Option<DateTime> dateAfter = new("--dateafter");
    private MultiOption<string> matchFilters = new("--match-filters");
    private Option<bool> noMatchFilters = new("--no-match-filters");
    private Option<string> breakMatchFilters = new("--break-match-filters");
    private Option<bool> noBreakMatchFilters = new("--no-break-match-filters");
    private Option<bool> noPlaylist = new("--no-playlist");
    private Option<bool> yesPlaylist = new("--yes-playlist");
    private Option<byte?> ageLimit = new("--age-limit");
    private Option<string> downloadArchive = new("--download-archive");
    private Option<bool> noDownloadArchive = new("--no-download-archive");
    private Option<int?> maxDownloads = new("--max-downloads");
    private Option<bool> breakOnExisting = new("--break-on-existing");
    private Option<bool> noBreakOnExisting = new("--no-break-on-existing");
    private Option<bool> breakPerInput = new("--break-per-input");
    private Option<bool> noBreakPerInput = new("--no-break-per-input");
    private Option<int?> skipPlaylistAfterErrors = new("--skip-playlist-after-errors");

    public string PlaylistItems { get => playlistItems.Value; set => playlistItems.Value = value; }
    public string MinFilesize { get => minFilesize.Value; set => minFilesize.Value = value; }
    public string MaxFilesize { get => maxFilesize.Value; set => maxFilesize.Value = value; }
    public DateTime Date { get => date.Value; set => date.Value = value; }
    public DateTime DateBefore { get => dateBefore.Value; set => dateBefore.Value = value; }
    public DateTime DateAfter { get => dateAfter.Value; set => dateAfter.Value = value; }
    public MultiValue<string> MatchFilters { get => matchFilters.Value; set => matchFilters.Value = value; }
    public bool NoMatchFilters { get => noMatchFilters.Value; set => noMatchFilters.Value = value; }
    public string BreakMatchFilters { get => breakMatchFilters.Value; set => breakMatchFilters.Value = value; }
    public bool NoBreakMatchFilters { get => noBreakMatchFilters.Value; set => noBreakMatchFilters.Value = value; }
    public bool NoPlaylist { get => noPlaylist.Value; set => noPlaylist.Value = value; }
    public bool YesPlaylist { get => yesPlaylist.Value; set => yesPlaylist.Value = value; }
    public byte? AgeLimit { get => ageLimit.Value; set => ageLimit.Value = value; }
    public string DownloadArchive { get => downloadArchive.Value; set => downloadArchive.Value = value; }
    public bool NoDownloadArchive { get => noDownloadArchive.Value; set => noDownloadArchive.Value = value; }
    public int? MaxDownloads { get => maxDownloads.Value; set => maxDownloads.Value = value; }
    public bool BreakOnExisting { get => breakOnExisting.Value; set => breakOnExisting.Value = value; }
    public bool NoBreakOnExisting { get => noBreakOnExisting.Value; set => noBreakOnExisting.Value = value; }
    public bool BreakPerInput { get => breakPerInput.Value; set => breakPerInput.Value = value; }
    public bool NoBreakPerInput { get => noBreakPerInput.Value; set => noBreakPerInput.Value = value; }
    public int? SkipPlaylistAfterErrors { get => skipPlaylistAfterErrors.Value; set => skipPlaylistAfterErrors.Value = value; }
}
