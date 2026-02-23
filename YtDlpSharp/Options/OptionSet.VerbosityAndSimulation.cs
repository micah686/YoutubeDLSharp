namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<bool> quiet = new("-q", "--quiet");
    private Option<bool> noQuiet = new("--no-quiet");
    private Option<bool> noWarnings = new("--no-warnings");
    private Option<bool> simulate = new("-s", "--simulate");
    private Option<bool> noSimulate = new("--no-simulate");
    private Option<bool> ignoreNoFormatsError = new("--ignore-no-formats-error");
    private Option<bool> noIgnoreNoFormatsError = new("--no-ignore-no-formats-error");
    private Option<bool> skipDownload = new("--skip-download", "--no-download");
    private MultiOption<string> print = new("-O", "--print");
    private MultiOption<string> printToFile = new("--print-to-file");
    private Option<bool> dumpJson = new("-j", "--dump-json");
    private Option<bool> dumpSingleJson = new("-J", "--dump-single-json");
    private Option<bool> forceWriteArchive = new("--force-write-archive", "--force-download-archive");
    private Option<bool> newline = new("--newline");
    private Option<bool> noProgress = new("--no-progress");
    private Option<bool> progress = new("--progress");
    private Option<bool> consoleTitle = new("--console-title");
    private Option<string> progressTemplate = new("--progress-template");
    private Option<string> progressDelta = new("--progress-delta");
    private Option<bool> verbose = new("-v", "--verbose");
    private Option<bool> dumpPages = new("--dump-pages");
    private Option<bool> writePages = new("--write-pages");
    private Option<bool> printTraffic = new("--print-traffic");

    public bool Quiet { get => quiet.Value; set => quiet.Value = value; }
    public bool NoQuiet { get => noQuiet.Value; set => noQuiet.Value = value; }
    public bool NoWarnings { get => noWarnings.Value; set => noWarnings.Value = value; }
    public bool Simulate { get => simulate.Value; set => simulate.Value = value; }
    public bool NoSimulate { get => noSimulate.Value; set => noSimulate.Value = value; }
    public bool IgnoreNoFormatsError { get => ignoreNoFormatsError.Value; set => ignoreNoFormatsError.Value = value; }
    public bool NoIgnoreNoFormatsError { get => noIgnoreNoFormatsError.Value; set => noIgnoreNoFormatsError.Value = value; }
    public bool SkipDownload { get => skipDownload.Value; set => skipDownload.Value = value; }
    public MultiValue<string> Print { get => print.Value; set => print.Value = value; }
    public MultiValue<string> PrintToFile { get => printToFile.Value; set => printToFile.Value = value; }
    public bool DumpJson { get => dumpJson.Value; set => dumpJson.Value = value; }
    public bool DumpSingleJson { get => dumpSingleJson.Value; set => dumpSingleJson.Value = value; }
    public bool ForceWriteArchive { get => forceWriteArchive.Value; set => forceWriteArchive.Value = value; }
    public bool Newline { get => newline.Value; set => newline.Value = value; }
    public bool NoProgress { get => noProgress.Value; set => noProgress.Value = value; }
    public bool Progress { get => progress.Value; set => progress.Value = value; }
    public bool ConsoleTitle { get => consoleTitle.Value; set => consoleTitle.Value = value; }
    public string ProgressTemplate { get => progressTemplate.Value; set => progressTemplate.Value = value; }
    public string ProgressDelta { get => progressDelta.Value; set => progressDelta.Value = value; }
    public bool Verbose { get => verbose.Value; set => verbose.Value = value; }
    public bool DumpPages { get => dumpPages.Value; set => dumpPages.Value = value; }
    public bool WritePages { get => writePages.Value; set => writePages.Value = value; }
    public bool PrintTraffic { get => printTraffic.Value; set => printTraffic.Value = value; }
}
