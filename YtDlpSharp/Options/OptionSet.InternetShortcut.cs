namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<bool> writeLink = new("--write-link");
    private Option<bool> writeUrlLink = new("--write-url-link");
    private Option<bool> writeWeblocLink = new("--write-webloc-link");
    private Option<bool> writeDesktopLink = new("--write-desktop-link");

    public bool WriteLink { get => writeLink.Value; set => writeLink.Value = value; }
    public bool WriteUrlLink { get => writeUrlLink.Value; set => writeUrlLink.Value = value; }
    public bool WriteWeblocLink { get => writeWeblocLink.Value; set => writeWeblocLink.Value = value; }
    public bool WriteDesktopLink { get => writeDesktopLink.Value; set => writeDesktopLink.Value = value; }
}
