using System.Runtime.CompilerServices;
using YtDlpSharp.Helpers;
using YtDlpSharp.Serialization;
using YtdlpPoco;

namespace YtDlpSharp;

public sealed class YtDlp : IYtDlp
{
    private static readonly Regex RgxFile = new(@"^outfile:\s\""?(.*)\""?", RegexOptions.Compiled);

    private readonly YtDlpConfiguration _config;
    private readonly ProcessRunner _runner;
    private readonly Func<YtDlpProcess> _processFactory;

    public YtDlp(YtDlpConfiguration config, IYtDlpProcess? processOverride = null)
    {
        _config = config;
        _runner = new ProcessRunner(config.MaxConcurrentProcesses);
        _processFactory = processOverride is YtDlpProcess p
            ? () => p
            : () => new YtDlpProcess(config.YtDlpPath);
    }

    public async Task<RunResult<string[]>> RunWithOptionsAsync(string[] urls, OptionSet options, CancellationToken ct = default)
    {
        var output = new List<string>();
        var process = _processFactory();
        process.OutputReceived += (_, e) => { if (e.Data is not null) output.Add(e.Data); };
        var (code, errors) = await _runner.RunThrottledAsync(process, urls, options, ct);
        return new RunResult<string[]>(code == 0, errors, output.ToArray());
    }

    public async Task<string> RunUpdateAsync()
    {
        string output = string.Empty;
        var process = _processFactory();
        process.OutputReceived += (_, e) => { if (e.Data is not null) output = e.Data; };
        await process.RunAsync(null, new OptionSet { Update = true });
        return output;
    }

    public async Task<RunResult<VideoData>> RunVideoDataFetchAsync(
        string url,
        CancellationToken ct = default,
        bool flat = true,
        bool fetchComments = false,
        OptionSet? overrideOptions = null)
    {
        var opts = GetDownloadOptions();
        opts.DumpSingleJson = true;
        opts.FlatPlaylist = flat;
        opts.WriteComments = fetchComments;
        if (overrideOptions is not null)
            opts = opts.OverrideOptions(overrideOptions);

        VideoData? videoData = null;
        var process = _processFactory();
        process.OutputReceived += (_, e) =>
        {
            if (e.Data is null) return;
            try
            {
                videoData = JsonSerializer.Deserialize(e.Data, YtDlpJsonContext.Default.VideoData);
            }
            catch (JsonException)
            {
                process.RedirectToError(e);
            }
        };

        var (code, errors) = await _runner.RunThrottledAsync(process, [url], opts, ct);
        return new RunResult<VideoData>(code == 0, errors, videoData!);
    }

    public async Task<RunResult<string>> RunVideoDownloadAsync(
        string url,
        string format = "bestvideo+bestaudio/best",
        DownloadMergeFormat mergeFormat = DownloadMergeFormat.Unspecified,
        VideoRecodeFormat recodeFormat = VideoRecodeFormat.None,
        CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null,
        IProgress<string>? output = null,
        OptionSet? overrideOptions = null)
    {
        var opts = GetDownloadOptions();
        opts.Format = format;
        opts.MergeOutputFormat = mergeFormat;
        opts.RecodeVideo = recodeFormat;
        if (overrideOptions is not null)
            opts = opts.OverrideOptions(overrideOptions);

        string outputFile = string.Empty;
        var process = _processFactory();
        output?.Report($"Arguments: {process.ConvertToArgs([url], opts)}\n");
        process.OutputReceived += (_, e) =>
        {
            if (e.Data is null) return;
            var match = RgxFile.Match(e.Data);
            if (match.Success)
            {
                outputFile = match.Groups[1].Value.Trim('"');
                progress?.Report(new DownloadProgress(DownloadState.Success, Data: outputFile));
            }
            output?.Report(e.Data);
        };

        var (code, errors) = await _runner.RunThrottledAsync(process, [url], opts, ct, progress);
        return new RunResult<string>(code == 0, errors, outputFile);
    }

    public async Task<RunResult<string>> RunAudioDownloadAsync(
        string url,
        AudioConversionFormat format = AudioConversionFormat.Best,
        CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null,
        IProgress<string>? output = null,
        OptionSet? overrideOptions = null)
    {
        var opts = GetDownloadOptions();
        opts.Format = "bestaudio/best";
        opts.ExtractAudio = true;
        opts.AudioFormat = format;
        if (overrideOptions is not null)
            opts = opts.OverrideOptions(overrideOptions);

        string outputFile = string.Empty;
        var process = _processFactory();
        output?.Report($"Arguments: {process.ConvertToArgs([url], opts)}\n");
        process.OutputReceived += (_, e) =>
        {
            if (e.Data is null) return;
            var match = RgxFile.Match(e.Data);
            if (match.Success)
            {
                outputFile = match.Groups[1].Value.Trim('"');
                progress?.Report(new DownloadProgress(DownloadState.Success, Data: outputFile));
            }
            output?.Report(e.Data);
        };

        var (code, errors) = await _runner.RunThrottledAsync(process, [url], opts, ct, progress);
        return new RunResult<string>(code == 0, errors, outputFile);
    }

    public async Task<RunResult<string[]>> RunVideoPlaylistDownloadAsync(
        string url,
        int? start = 1, int? end = null,
        int[]? items = null,
        string format = "bestvideo+bestaudio/best",
        VideoRecodeFormat recodeFormat = VideoRecodeFormat.None,
        CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null,
        IProgress<string>? output = null,
        OptionSet? overrideOptions = null)
    {
        var opts = GetDownloadOptions();
        opts.NoPlaylist = false;
        ApplyPlaylistItems(opts, start, end, items);
        opts.Format = format;
        opts.RecodeVideo = recodeFormat;
        if (overrideOptions is not null)
            opts = opts.OverrideOptions(overrideOptions);

        var outputFiles = new List<string>();
        var process = _processFactory();
        output?.Report($"Arguments: {process.ConvertToArgs([url], opts)}\n");
        process.OutputReceived += (_, e) =>
        {
            if (e.Data is null) return;
            var match = RgxFile.Match(e.Data);
            if (match.Success)
            {
                var file = match.Groups[1].Value.Trim('"');
                outputFiles.Add(file);
                progress?.Report(new DownloadProgress(DownloadState.Success, Data: file));
            }
            output?.Report(e.Data);
        };

        var (code, errors) = await _runner.RunThrottledAsync(process, [url], opts, ct, progress);
        return new RunResult<string[]>(code == 0, errors, outputFiles.ToArray());
    }

    public async Task<RunResult<string[]>> RunAudioPlaylistDownloadAsync(
        string url,
        int? start = 1, int? end = null,
        int[]? items = null,
        AudioConversionFormat format = AudioConversionFormat.Best,
        CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null,
        IProgress<string>? output = null,
        OptionSet? overrideOptions = null)
    {
        var opts = GetDownloadOptions();
        opts.NoPlaylist = false;
        ApplyPlaylistItems(opts, start, end, items);
        opts.Format = "bestaudio/best";
        opts.ExtractAudio = true;
        opts.AudioFormat = format;
        if (overrideOptions is not null)
            opts = opts.OverrideOptions(overrideOptions);

        var outputFiles = new List<string>();
        var process = _processFactory();
        output?.Report($"Arguments: {process.ConvertToArgs([url], opts)}\n");
        process.OutputReceived += (_, e) =>
        {
            if (e.Data is null) return;
            var match = RgxFile.Match(e.Data);
            if (match.Success)
            {
                var file = match.Groups[1].Value.Trim('"');
                outputFiles.Add(file);
                progress?.Report(new DownloadProgress(DownloadState.Success, Data: file));
            }
            output?.Report(e.Data);
        };

        var (code, errors) = await _runner.RunThrottledAsync(process, [url], opts, ct, progress);
        return new RunResult<string[]>(code == 0, errors, outputFiles.ToArray());
    }

    public async IAsyncEnumerable<RunResult<string>> StreamPlaylistDownloadAsync(
        string url,
        string format = "bestvideo+bestaudio/best",
        VideoRecodeFormat recodeFormat = VideoRecodeFormat.None,
        [EnumeratorCancellation] CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null,
        OptionSet? overrideOptions = null)
    {
        var opts = GetDownloadOptions();
        opts.NoPlaylist = false;
        opts.Format = format;
        opts.RecodeVideo = recodeFormat;
        if (overrideOptions is not null)
            opts = opts.OverrideOptions(overrideOptions);

        var channel = Channel.CreateUnbounded<string>();
        var process = _processFactory();
        process.OutputReceived += (_, e) =>
        {
            if (e.Data is null) return;
            var match = RgxFile.Match(e.Data);
            if (match.Success)
            {
                var file = match.Groups[1].Value.Trim('"');
                channel.Writer.TryWrite(file);
                progress?.Report(new DownloadProgress(DownloadState.Success, Data: file));
            }
        };

        var runTask = Task.Run(async () =>
        {
            var (code, errors) = await _runner.RunThrottledAsync(process, [url], opts, ct, progress);
            channel.Writer.Complete();
            return (code, errors);
        }, ct);

        await foreach (var file in channel.Reader.ReadAllAsync(ct))
        {
            yield return new RunResult<string>(true, [], file);
        }

        var (exitCode, finalErrors) = await runTask;
        if (exitCode != 0)
            yield return new RunResult<string>(false, finalErrors, string.Empty);
    }

    public async IAsyncEnumerable<DownloadProgress> StreamProgressAsync(
        string url,
        OptionSet options,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var channel = Channel.CreateUnbounded<DownloadProgress>();
        var progressReporter = new Progress<DownloadProgress>(p => channel.Writer.TryWrite(p));

        var process = _processFactory();
        var runTask = Task.Run(async () =>
        {
            await _runner.RunThrottledAsync(process, [url], options, ct, progressReporter);
            channel.Writer.Complete();
        }, ct);

        await foreach (var p in channel.Reader.ReadAllAsync(ct))
        {
            yield return p;
        }

        await runTask;
    }

    public async IAsyncEnumerable<RunResult<string>> RunBatchAsync(
        string[] urls,
        OptionSet options,
        [EnumeratorCancellation] CancellationToken ct = default,
        IProgress<DownloadProgress>? progress = null)
    {
        var tasks = urls.Select(url => RunVideoDownloadAsync(url, overrideOptions: options, ct: ct, progress: progress)).ToList();

        await foreach (var completedTask in Task.WhenEach(tasks))
        {
            yield return await completedTask;
        }
    }

    private OptionSet GetDownloadOptions() => new()
    {
        IgnoreErrors = _config.IgnoreDownloadErrors,
        IgnoreConfig = true,
        NoPlaylist = true,
        Downloader = "m3u8:native",
        DownloaderArgs = "ffmpeg:-nostats -loglevel 0",
        Output = Path.Combine(_config.OutputFolder, _config.OutputFileTemplate),
        RestrictFilenames = _config.RestrictFilenames,
        ForceOverwrites = _config.OverwriteFiles,
        NoOverwrites = !_config.OverwriteFiles,
        NoPart = true,
        FfmpegLocation = Utils.GetFullPath(_config.FfmpegPath) ?? _config.FfmpegPath,
        Progress = true,
        Print = "after_move:outfile: %(filepath)s"
    };

    private static void ApplyPlaylistItems(OptionSet opts, int? start, int? end, int[]? items)
    {
        if (items is not null)
            opts.PlaylistItems = string.Join(",", items);
        else if (start is not null || end is not null)
            opts.PlaylistItems = $"{start}:{end}";
    }
}
