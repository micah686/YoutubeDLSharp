using System.Globalization;
using YtDlpSharp.Helpers;

namespace YtDlpSharp;

public sealed class YtDlpProcess : IYtDlpProcess
{
    private static readonly Regex RgxPlaylist = new(@"Downloading video (\d+) of (\d+)", RegexOptions.Compiled);
    private static readonly Regex RgxProgress = new(
        @"\[download\]\s+(?:(?<percent>[\d\.]+)%(?:\s+of\s+\~?\s*(?<total>[\d\.\w]+))?\s+at\s+(?:(?<speed>[\d\.\w]+\/s)|[\w\s]+)\s+ETA\s(?<eta>[\d\:]+))?",
        RegexOptions.Compiled);
    private static readonly Regex RgxPost = new(@"\[(\w+)\]\s+", RegexOptions.Compiled);

    public string ExecutablePath { get; set; }

    public event EventHandler<DataReceivedEventArgs>? OutputReceived;
    public event EventHandler<DataReceivedEventArgs>? ErrorReceived;

    public YtDlpProcess(string executablePath = "yt-dlp")
    {
        ExecutablePath = executablePath;
    }

    public string ConvertToArgs(string[] urls, OptionSet options) =>
        $"{options} -- {(urls is not null ? string.Join(" ", urls.Select(s => $"\"{s}\"")) : string.Empty)}";

    internal void RedirectToError(DataReceivedEventArgs e) =>
        ErrorReceived?.Invoke(this, e);

    public async Task<int> RunAsync(
        string[]? urls,
        OptionSet options,
        CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null)
    {
        var tcs = new TaskCompletionSource<int>();
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
            FileName = ExecutablePath,
            Arguments = ConvertToArgs(urls ?? [], options)
        };

        process.EnableRaisingEvents = true;
        process.StartInfo = startInfo;

        var tcsOut = new TaskCompletionSource<bool>();
        var tcsError = new TaskCompletionSource<bool>();
        bool isDownloading = false;

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data is null)
            {
                tcsOut.TrySetResult(true);
                return;
            }

            Match match;
            if ((match = RgxProgress.Match(e.Data)).Success)
            {
                if (match.Groups.Count > 1 && match.Groups[1].Length > 0)
                {
                    float progValue = float.Parse(match.Groups["percent"].Value, CultureInfo.InvariantCulture) / 100.0f;
                    string? total = match.Groups["total"] is { Success: true } tg ? tg.Value : null;
                    string? speed = match.Groups["speed"] is { Success: true } sg ? sg.Value : null;
                    string? eta = match.Groups["eta"] is { Success: true } eg ? eg.Value : null;
                    progress?.Report(new DownloadProgress(DownloadState.Downloading, progValue, total, speed, eta));
                }
                else
                {
                    progress?.Report(new DownloadProgress(DownloadState.Downloading));
                }
                isDownloading = true;
            }
            else if ((match = RgxPlaylist.Match(e.Data)).Success)
            {
                var index = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                progress?.Report(new DownloadProgress(DownloadState.PreProcessing, VideoIndex: index));
                isDownloading = false;
            }
            else if (isDownloading && RgxPost.Match(e.Data).Success)
            {
                progress?.Report(new DownloadProgress(DownloadState.PostProcessing, 1));
                isDownloading = false;
            }

            Debug.WriteLine($"[yt-dlp] {e.Data}");
            OutputReceived?.Invoke(this, e);
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is null)
            {
                tcsError.TrySetResult(true);
                return;
            }
            Debug.WriteLine($"[yt-dlp ERROR] {e.Data}");
            progress?.Report(new DownloadProgress(DownloadState.Error, Data: e.Data));
            ErrorReceived?.Invoke(this, e);
        };

        process.Exited += async (_, _) =>
        {
            await tcsOut.Task;
            await tcsError.Task;
            tcs.TrySetResult(process.ExitCode);
            process.Dispose();
        };

        ct.Register(() =>
        {
            if (!tcs.Task.IsCompleted)
                tcs.TrySetCanceled();
            try
            {
                if (!process.HasExited)
                    process.KillTree();
            }
            catch { }
        });

        Debug.WriteLine($"[yt-dlp] Arguments: {process.StartInfo.Arguments}");
        if (!await Task.Run(() => process.Start(), ct))
            tcs.TrySetException(new InvalidOperationException("Failed to start yt-dlp process."));

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        progress?.Report(new DownloadProgress(DownloadState.PreProcessing));
        return await tcs.Task;
    }
}
