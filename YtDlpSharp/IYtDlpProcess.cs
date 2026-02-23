namespace YtDlpSharp;

public interface IYtDlpProcess
{
    string ExecutablePath { get; set; }
    event EventHandler<DataReceivedEventArgs> OutputReceived;
    event EventHandler<DataReceivedEventArgs> ErrorReceived;
    string ConvertToArgs(string[] urls, OptionSet options);
    Task<int> RunAsync(string[]? urls, OptionSet options, CancellationToken ct = default, IProgress<DownloadProgress>? progress = null);
}
