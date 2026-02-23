namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<bool> help = new("-h", "--help");
    private Option<bool> version = new("--version");
    private Option<bool> update = new("-U", "--update");
    private Option<bool> noUpdate = new("--no-update");
    private Option<string> updateTo = new("--update-to");
    private Option<bool> ignoreErrors = new("-i", "--ignore-errors");
    private Option<bool> noAbortOnError = new("--no-abort-on-error");
    private Option<bool> abortOnError = new("--abort-on-error", "--no-ignore-errors");
    private Option<bool> listExtractors = new("--list-extractors");
    private Option<bool> extractorDescriptions = new("--extractor-descriptions");
    private Option<string> useExtractors = new("--use-extractors", "--ies");
    private Option<string> defaultSearch = new("--default-search");
    private Option<bool> ignoreConfig = new("--ignore-config", "--no-config");
    private Option<bool> noConfigLocations = new("--no-config-locations");
    private MultiOption<string> configLocations = new("--config-locations");
    private MultiOption<string> pluginDirs = new("--plugin-dirs");
    private Option<bool> noPluginDirs = new("--no-plugin-dirs");
    private MultiOption<string> jsRuntimes = new("--js-runtimes");
    private Option<bool> noJsRuntimes = new("--no-js-runtimes");
    private MultiOption<string> remoteComponents = new("--remote-components");
    private Option<bool> noRemoteComponents = new("--no-remote-components");
    private Option<bool> flatPlaylist = new("--flat-playlist");
    private Option<bool> noFlatPlaylist = new("--no-flat-playlist");
    private Option<bool> liveFromStart = new("--live-from-start");
    private Option<bool> noLiveFromStart = new("--no-live-from-start");
    private Option<string> waitForVideo = new("--wait-for-video");
    private Option<bool> noWaitForVideo = new("--no-wait-for-video");
    private Option<bool> markWatched = new("--mark-watched");
    private Option<bool> noMarkWatched = new("--no-mark-watched");
    private MultiOption<string> color = new("--color");
    private Option<string> compatOptions = new("--compat-options");
    private MultiOption<string> alias = new("--alias");
    private MultiOption<string> presetAlias = new("-t", "--preset-alias");

    public bool Help { get => help.Value; set => help.Value = value; }
    public bool Version { get => version.Value; set => version.Value = value; }
    public bool Update { get => update.Value; set => update.Value = value; }
    public bool NoUpdate { get => noUpdate.Value; set => noUpdate.Value = value; }
    public string UpdateTo { get => updateTo.Value; set => updateTo.Value = value; }
    public bool IgnoreErrors { get => ignoreErrors.Value; set => ignoreErrors.Value = value; }
    public bool NoAbortOnError { get => noAbortOnError.Value; set => noAbortOnError.Value = value; }
    public bool AbortOnError { get => abortOnError.Value; set => abortOnError.Value = value; }
    public bool ListExtractors { get => listExtractors.Value; set => listExtractors.Value = value; }
    public bool ExtractorDescriptions { get => extractorDescriptions.Value; set => extractorDescriptions.Value = value; }
    public string UseExtractors { get => useExtractors.Value; set => useExtractors.Value = value; }
    public string DefaultSearch { get => defaultSearch.Value; set => defaultSearch.Value = value; }
    public bool IgnoreConfig { get => ignoreConfig.Value; set => ignoreConfig.Value = value; }
    public bool NoConfigLocations { get => noConfigLocations.Value; set => noConfigLocations.Value = value; }
    public MultiValue<string> ConfigLocations { get => configLocations.Value; set => configLocations.Value = value; }
    public MultiValue<string> PluginDirs { get => pluginDirs.Value; set => pluginDirs.Value = value; }
    public bool NoPluginDirs { get => noPluginDirs.Value; set => noPluginDirs.Value = value; }
    public MultiValue<string> JsRuntimes { get => jsRuntimes.Value; set => jsRuntimes.Value = value; }
    public bool NoJsRuntimes { get => noJsRuntimes.Value; set => noJsRuntimes.Value = value; }
    public MultiValue<string> RemoteComponents { get => remoteComponents.Value; set => remoteComponents.Value = value; }
    public bool NoRemoteComponents { get => noRemoteComponents.Value; set => noRemoteComponents.Value = value; }
    public bool FlatPlaylist { get => flatPlaylist.Value; set => flatPlaylist.Value = value; }
    public bool NoFlatPlaylist { get => noFlatPlaylist.Value; set => noFlatPlaylist.Value = value; }
    public bool LiveFromStart { get => liveFromStart.Value; set => liveFromStart.Value = value; }
    public bool NoLiveFromStart { get => noLiveFromStart.Value; set => noLiveFromStart.Value = value; }
    public string WaitForVideo { get => waitForVideo.Value; set => waitForVideo.Value = value; }
    public bool NoWaitForVideo { get => noWaitForVideo.Value; set => noWaitForVideo.Value = value; }
    public bool MarkWatched { get => markWatched.Value; set => markWatched.Value = value; }
    public bool NoMarkWatched { get => noMarkWatched.Value; set => noMarkWatched.Value = value; }
    public MultiValue<string> Color { get => color.Value; set => color.Value = value; }
    public string CompatOptions { get => compatOptions.Value; set => compatOptions.Value = value; }
    public MultiValue<string> Alias { get => alias.Value; set => alias.Value = value; }
    public MultiValue<string> PresetAlias { get => presetAlias.Value; set => presetAlias.Value = value; }
}
