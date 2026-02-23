namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<int?> concurrentFragments = new("-N", "--concurrent-fragments");
    private Option<long?> limitRate = new("-r", "--limit-rate");
    private Option<long?> throttledRate = new("--throttled-rate");
    private Option<int?> retries = new("-R", "--retries");
    private Option<int?> fileAccessRetries = new("--file-access-retries");
    private Option<int?> fragmentRetries = new("--fragment-retries");
    private MultiOption<string> retrySleep = new("--retry-sleep");
    private Option<bool> skipUnavailableFragments = new("--skip-unavailable-fragments", "--no-abort-on-unavailable-fragments");
    private Option<bool> abortOnUnavailableFragments = new("--abort-on-unavailable-fragments", "--no-skip-unavailable-fragments");
    private Option<bool> keepFragments = new("--keep-fragments");
    private Option<bool> noKeepFragments = new("--no-keep-fragments");
    private Option<long?> bufferSize = new("--buffer-size");
    private Option<bool> resizeBuffer = new("--resize-buffer");
    private Option<bool> noResizeBuffer = new("--no-resize-buffer");
    private Option<long?> httpChunkSize = new("--http-chunk-size");
    private Option<bool> playlistRandom = new("--playlist-random");
    private Option<bool> lazyPlaylist = new("--lazy-playlist");
    private Option<bool> noLazyPlaylist = new("--no-lazy-playlist");
    private Option<bool> hlsUseMpegts = new("--hls-use-mpegts");
    private Option<bool> noHlsUseMpegts = new("--no-hls-use-mpegts");
    private MultiOption<string> downloadSections = new("--download-sections");
    private MultiOption<string> downloader = new("--downloader", "--external-downloader");
    private MultiOption<string> downloaderArgs = new("--downloader-args", "--external-downloader-args");

    public int? ConcurrentFragments { get => concurrentFragments.Value; set => concurrentFragments.Value = value; }
    public long? LimitRate { get => limitRate.Value; set => limitRate.Value = value; }
    public long? ThrottledRate { get => throttledRate.Value; set => throttledRate.Value = value; }
    public int? Retries { get => retries.Value; set => retries.Value = value; }
    public int? FileAccessRetries { get => fileAccessRetries.Value; set => fileAccessRetries.Value = value; }
    public int? FragmentRetries { get => fragmentRetries.Value; set => fragmentRetries.Value = value; }
    public MultiValue<string> RetrySleep { get => retrySleep.Value; set => retrySleep.Value = value; }
    public bool SkipUnavailableFragments { get => skipUnavailableFragments.Value; set => skipUnavailableFragments.Value = value; }
    public bool AbortOnUnavailableFragments { get => abortOnUnavailableFragments.Value; set => abortOnUnavailableFragments.Value = value; }
    public bool KeepFragments { get => keepFragments.Value; set => keepFragments.Value = value; }
    public bool NoKeepFragments { get => noKeepFragments.Value; set => noKeepFragments.Value = value; }
    public long? BufferSize { get => bufferSize.Value; set => bufferSize.Value = value; }
    public bool ResizeBuffer { get => resizeBuffer.Value; set => resizeBuffer.Value = value; }
    public bool NoResizeBuffer { get => noResizeBuffer.Value; set => noResizeBuffer.Value = value; }
    public long? HttpChunkSize { get => httpChunkSize.Value; set => httpChunkSize.Value = value; }
    public bool PlaylistRandom { get => playlistRandom.Value; set => playlistRandom.Value = value; }
    public bool LazyPlaylist { get => lazyPlaylist.Value; set => lazyPlaylist.Value = value; }
    public bool NoLazyPlaylist { get => noLazyPlaylist.Value; set => noLazyPlaylist.Value = value; }
    public bool HlsUseMpegts { get => hlsUseMpegts.Value; set => hlsUseMpegts.Value = value; }
    public bool NoHlsUseMpegts { get => noHlsUseMpegts.Value; set => noHlsUseMpegts.Value = value; }
    public MultiValue<string> DownloadSections { get => downloadSections.Value; set => downloadSections.Value = value; }
    public MultiValue<string> Downloader { get => downloader.Value; set => downloader.Value = value; }
    public MultiValue<string> DownloaderArgs { get => downloaderArgs.Value; set => downloaderArgs.Value = value; }
}
