namespace YtDlpSharp.Models;

public sealed record DownloadProgress(
    DownloadState State,
    float Progress = 0,
    string? TotalDownloadSize = null,
    string? DownloadSpeed = null,
    string? Eta = null,
    int VideoIndex = 1,
    string? Data = null);
