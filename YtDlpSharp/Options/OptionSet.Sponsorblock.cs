namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<string> sponsorblockMark = new("--sponsorblock-mark");
    private Option<string> sponsorblockRemove = new("--sponsorblock-remove");
    private Option<string> sponsorblockChapterTitle = new("--sponsorblock-chapter-title");
    private Option<bool> noSponsorblock = new("--no-sponsorblock");
    private Option<string> sponsorblockApi = new("--sponsorblock-api");

    public string SponsorblockMark { get => sponsorblockMark.Value; set => sponsorblockMark.Value = value; }
    public string SponsorblockRemove { get => sponsorblockRemove.Value; set => sponsorblockRemove.Value = value; }
    public string SponsorblockChapterTitle { get => sponsorblockChapterTitle.Value; set => sponsorblockChapterTitle.Value = value; }
    public bool NoSponsorblock { get => noSponsorblock.Value; set => noSponsorblock.Value = value; }
    public string SponsorblockApi { get => sponsorblockApi.Value; set => sponsorblockApi.Value = value; }
}
