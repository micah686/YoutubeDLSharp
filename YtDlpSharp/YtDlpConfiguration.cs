namespace YtDlpSharp;

public sealed record YtDlpConfiguration
{
    public required string YtDlpPath { get; init; }
    public string FfmpegPath { get; init; } = "ffmpeg";
    public string OutputFolder { get; init; } = Environment.CurrentDirectory;
    public string OutputFileTemplate { get; init; } = "%(title)s [%(id)s].%(ext)s";
    public bool RestrictFilenames { get; init; }
    public bool OverwriteFiles { get; init; } = true;
    public bool IgnoreDownloadErrors { get; init; } = true;
    public int MaxConcurrentProcesses { get; init; } = 4;
}
