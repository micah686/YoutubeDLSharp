namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<string> encoding = new("--encoding");
    private Option<bool> legacyServerConnect = new("--legacy-server-connect");
    private Option<bool> noCheckCertificates = new("--no-check-certificates");
    private Option<bool> preferInsecure = new("--prefer-insecure");
    private MultiOption<string> addHeaders = new("--add-headers");
    private Option<bool> bidiWorkaround = new("--bidi-workaround");
    private Option<int?> sleepRequests = new("--sleep-requests");
    private Option<int?> sleepInterval = new("--sleep-interval", "--min-sleep-interval");
    private Option<int?> maxSleepInterval = new("--max-sleep-interval");
    private Option<int?> sleepSubtitles = new("--sleep-subtitles");

    public string Encoding { get => encoding.Value; set => encoding.Value = value; }
    public bool LegacyServerConnect { get => legacyServerConnect.Value; set => legacyServerConnect.Value = value; }
    public bool NoCheckCertificates { get => noCheckCertificates.Value; set => noCheckCertificates.Value = value; }
    public bool PreferInsecure { get => preferInsecure.Value; set => preferInsecure.Value = value; }
    public MultiValue<string> AddHeaders { get => addHeaders.Value; set => addHeaders.Value = value; }
    public bool BidiWorkaround { get => bidiWorkaround.Value; set => bidiWorkaround.Value = value; }
    public int? SleepRequests { get => sleepRequests.Value; set => sleepRequests.Value = value; }
    public int? SleepInterval { get => sleepInterval.Value; set => sleepInterval.Value = value; }
    public int? MaxSleepInterval { get => maxSleepInterval.Value; set => maxSleepInterval.Value = value; }
    public int? SleepSubtitles { get => sleepSubtitles.Value; set => sleepSubtitles.Value = value; }
}
