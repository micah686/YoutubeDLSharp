namespace YtDlpSharp.Models;

public enum DownloadState
{
    None = 0,
    PreProcessing = 1,
    Downloading = 2,
    PostProcessing = 3,
    Error = 4,
    Success = 5
}
