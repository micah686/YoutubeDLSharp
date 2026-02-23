using System.Runtime.CompilerServices;
using YtdlpPoco;

namespace YtDlpSharp;

public interface IYtDlp
{
    Task<RunResult<VideoData>> RunVideoDataFetchAsync(string url,
        CancellationToken ct = default,
        bool flat = true,
        bool fetchComments = false,
        OptionSet? overrideOptions = null);

    Task<RunResult<string>> RunVideoDownloadAsync(string url,
        string format = "bestvideo+bestaudio/best",
        DownloadMergeFormat mergeFormat = DownloadMergeFormat.Unspecified,
        VideoRecodeFormat recodeFormat = VideoRecodeFormat.None,
        CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null,
        IProgress<string>? output = null,
        OptionSet? overrideOptions = null);

    Task<RunResult<string>> RunAudioDownloadAsync(string url,
        AudioConversionFormat format = AudioConversionFormat.Best,
        CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null,
        IProgress<string>? output = null,
        OptionSet? overrideOptions = null);

    Task<RunResult<string[]>> RunVideoPlaylistDownloadAsync(string url,
        int? start = 1, int? end = null,
        int[]? items = null,
        string format = "bestvideo+bestaudio/best",
        VideoRecodeFormat recodeFormat = VideoRecodeFormat.None,
        CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null,
        IProgress<string>? output = null,
        OptionSet? overrideOptions = null);

    Task<RunResult<string[]>> RunAudioPlaylistDownloadAsync(string url,
        int? start = 1, int? end = null,
        int[]? items = null,
        AudioConversionFormat format = AudioConversionFormat.Best,
        CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null,
        IProgress<string>? output = null,
        OptionSet? overrideOptions = null);

    Task<RunResult<string[]>> RunWithOptionsAsync(string[] urls, OptionSet options, CancellationToken ct = default);

    Task<string> RunUpdateAsync();

    IAsyncEnumerable<RunResult<string>> StreamPlaylistDownloadAsync(string url,
        string format = "bestvideo+bestaudio/best",
        VideoRecodeFormat recodeFormat = VideoRecodeFormat.None,
        CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null,
        OptionSet? overrideOptions = null);

    IAsyncEnumerable<DownloadProgress> StreamProgressAsync(string url,
        OptionSet options,
        CancellationToken ct = default);

    IAsyncEnumerable<RunResult<string>> RunBatchAsync(string[] urls,
        OptionSet options,
        CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null);
}
