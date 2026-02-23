namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<string> batchFile = new("-a", "--batch-file");
    private Option<bool> noBatchFile = new("--no-batch-file");
    private MultiOption<string> paths = new("-P", "--paths");
    private Option<string> output = new("-o", "--output");
    private Option<string> outputNaPlaceholder = new("--output-na-placeholder");
    private Option<bool> restrictFilenames = new("--restrict-filenames");
    private Option<bool> noRestrictFilenames = new("--no-restrict-filenames");
    private Option<bool> windowsFilenames = new("--windows-filenames");
    private Option<bool> noWindowsFilenames = new("--no-windows-filenames");
    private Option<int?> trimFilenames = new("--trim-filenames");
    private Option<bool> noOverwrites = new("-w", "--no-overwrites");
    private Option<bool> forceOverwrites = new("--force-overwrites");
    private Option<bool> noForceOverwrites = new("--no-force-overwrites");
    private Option<bool> doContinue = new("-c", "--continue");
    private Option<bool> noContinue = new("--no-continue");
    private Option<bool> part = new("--part");
    private Option<bool> noPart = new("--no-part");
    private Option<bool> mtime = new("--mtime");
    private Option<bool> noMtime = new("--no-mtime");
    private Option<bool> writeDescription = new("--write-description");
    private Option<bool> noWriteDescription = new("--no-write-description");
    private Option<bool> writeInfoJson = new("--write-info-json");
    private Option<bool> noWriteInfoJson = new("--no-write-info-json");
    private Option<bool> writePlaylistMetafiles = new("--write-playlist-metafiles");
    private Option<bool> noWritePlaylistMetafiles = new("--no-write-playlist-metafiles");
    private Option<bool> cleanInfoJson = new("--clean-info-json");
    private Option<bool> noCleanInfoJson = new("--no-clean-info-json");
    private Option<bool> writeComments = new("--write-comments", "--get-comments");
    private Option<bool> noWriteComments = new("--no-write-comments", "--no-get-comments");
    private Option<string> loadInfoJson = new("--load-info-json");
    private Option<string> cookies = new("--cookies");
    private Option<bool> noCookies = new("--no-cookies");
    private Option<string> cookiesFromBrowser = new("--cookies-from-browser");
    private Option<bool> noCookiesFromBrowser = new("--no-cookies-from-browser");
    private Option<string> cacheDir = new("--cache-dir");
    private Option<bool> noCacheDir = new("--no-cache-dir");
    private Option<bool> removeCacheDir = new("--rm-cache-dir");

    public string BatchFile { get => batchFile.Value; set => batchFile.Value = value; }
    public bool NoBatchFile { get => noBatchFile.Value; set => noBatchFile.Value = value; }
    public MultiValue<string> Paths { get => paths.Value; set => paths.Value = value; }
    public string Output { get => output.Value; set => output.Value = value; }
    public string OutputNaPlaceholder { get => outputNaPlaceholder.Value; set => outputNaPlaceholder.Value = value; }
    public bool RestrictFilenames { get => restrictFilenames.Value; set => restrictFilenames.Value = value; }
    public bool NoRestrictFilenames { get => noRestrictFilenames.Value; set => noRestrictFilenames.Value = value; }
    public bool WindowsFilenames { get => windowsFilenames.Value; set => windowsFilenames.Value = value; }
    public bool NoWindowsFilenames { get => noWindowsFilenames.Value; set => noWindowsFilenames.Value = value; }
    public int? TrimFilenames { get => trimFilenames.Value; set => trimFilenames.Value = value; }
    public bool NoOverwrites { get => noOverwrites.Value; set => noOverwrites.Value = value; }
    public bool ForceOverwrites { get => forceOverwrites.Value; set => forceOverwrites.Value = value; }
    public bool NoForceOverwrites { get => noForceOverwrites.Value; set => noForceOverwrites.Value = value; }
    public bool Continue { get => doContinue.Value; set => doContinue.Value = value; }
    public bool NoContinue { get => noContinue.Value; set => noContinue.Value = value; }
    public bool Part { get => part.Value; set => part.Value = value; }
    public bool NoPart { get => noPart.Value; set => noPart.Value = value; }
    public bool Mtime { get => mtime.Value; set => mtime.Value = value; }
    public bool NoMtime { get => noMtime.Value; set => noMtime.Value = value; }
    public bool WriteDescription { get => writeDescription.Value; set => writeDescription.Value = value; }
    public bool NoWriteDescription { get => noWriteDescription.Value; set => noWriteDescription.Value = value; }
    public bool WriteInfoJson { get => writeInfoJson.Value; set => writeInfoJson.Value = value; }
    public bool NoWriteInfoJson { get => noWriteInfoJson.Value; set => noWriteInfoJson.Value = value; }
    public bool WritePlaylistMetafiles { get => writePlaylistMetafiles.Value; set => writePlaylistMetafiles.Value = value; }
    public bool NoWritePlaylistMetafiles { get => noWritePlaylistMetafiles.Value; set => noWritePlaylistMetafiles.Value = value; }
    public bool CleanInfoJson { get => cleanInfoJson.Value; set => cleanInfoJson.Value = value; }
    public bool NoCleanInfoJson { get => noCleanInfoJson.Value; set => noCleanInfoJson.Value = value; }
    public bool WriteComments { get => writeComments.Value; set => writeComments.Value = value; }
    public bool NoWriteComments { get => noWriteComments.Value; set => noWriteComments.Value = value; }
    public string LoadInfoJson { get => loadInfoJson.Value; set => loadInfoJson.Value = value; }
    public string Cookies { get => cookies.Value; set => cookies.Value = value; }
    public bool NoCookies { get => noCookies.Value; set => noCookies.Value = value; }
    public string CookiesFromBrowser { get => cookiesFromBrowser.Value; set => cookiesFromBrowser.Value = value; }
    public bool NoCookiesFromBrowser { get => noCookiesFromBrowser.Value; set => noCookiesFromBrowser.Value = value; }
    public string CacheDir { get => cacheDir.Value; set => cacheDir.Value = value; }
    public bool NoCacheDir { get => noCacheDir.Value; set => noCacheDir.Value = value; }
    public bool RemoveCacheDir { get => removeCacheDir.Value; set => removeCacheDir.Value = value; }
}
