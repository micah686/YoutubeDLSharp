namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<bool> writeSubs = new("--write-subs");
    private Option<bool> noWriteSubs = new("--no-write-subs");
    private Option<bool> writeAutoSubs = new("--write-auto-subs", "--write-automatic-subs");
    private Option<bool> noWriteAutoSubs = new("--no-write-auto-subs", "--no-write-automatic-subs");
    private Option<bool> listSubs = new("--list-subs");
    private Option<string> subFormat = new("--sub-format");
    private Option<string> subLangs = new("--sub-langs");

    public bool WriteSubs { get => writeSubs.Value; set => writeSubs.Value = value; }
    public bool NoWriteSubs { get => noWriteSubs.Value; set => noWriteSubs.Value = value; }
    public bool WriteAutoSubs { get => writeAutoSubs.Value; set => writeAutoSubs.Value = value; }
    public bool NoWriteAutoSubs { get => noWriteAutoSubs.Value; set => noWriteAutoSubs.Value = value; }
    public bool ListSubs { get => listSubs.Value; set => listSubs.Value = value; }
    public string SubFormat { get => subFormat.Value; set => subFormat.Value = value; }
    public string SubLangs { get => subLangs.Value; set => subLangs.Value = value; }
}
