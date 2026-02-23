namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<bool> extractAudio = new("-x", "--extract-audio");
    private Option<AudioConversionFormat> audioFormat = new("--audio-format");
    private Option<byte?> audioQuality = new("--audio-quality");
    private Option<string> remuxVideo = new("--remux-video");
    private Option<VideoRecodeFormat> recodeVideo = new("--recode-video");
    private MultiOption<string> postprocessorArgs = new("--postprocessor-args", "--ppa");
    private Option<bool> keepVideo = new("-k", "--keep-video");
    private Option<bool> noKeepVideo = new("--no-keep-video");
    private Option<bool> postOverwrites = new("--post-overwrites");
    private Option<bool> noPostOverwrites = new("--no-post-overwrites");
    private Option<bool> embedSubs = new("--embed-subs");
    private Option<bool> noEmbedSubs = new("--no-embed-subs");
    private Option<bool> embedThumbnail = new("--embed-thumbnail");
    private Option<bool> noEmbedThumbnail = new("--no-embed-thumbnail");
    private Option<bool> embedMetadata = new("--embed-metadata", "--add-metadata");
    private Option<bool> noEmbedMetadata = new("--no-embed-metadata", "--no-add-metadata");
    private Option<bool> embedChapters = new("--embed-chapters", "--add-chapters");
    private Option<bool> noEmbedChapters = new("--no-embed-chapters", "--no-add-chapters");
    private Option<bool> embedInfoJson = new("--embed-info-json");
    private Option<bool> noEmbedInfoJson = new("--no-embed-info-json");
    private Option<string> parseMetadata = new("--parse-metadata");
    private MultiOption<string> replaceInMetadata = new("--replace-in-metadata");
    private Option<bool> xattrs = new("--xattrs");
    private Option<string> concatPlaylist = new("--concat-playlist");
    private Option<string> fixup = new("--fixup");
    private Option<string> ffmpegLocation = new("--ffmpeg-location");
    private MultiOption<string> exec = new("--exec");
    private Option<bool> noExec = new("--no-exec");
    private Option<string> convertSubs = new("--convert-subs");
    private Option<string> convertThumbnails = new("--convert-thumbnails");
    private Option<bool> splitChapters = new("--split-chapters");
    private Option<bool> noSplitChapters = new("--no-split-chapters");
    private MultiOption<string> removeChapters = new("--remove-chapters");
    private Option<bool> noRemoveChapters = new("--no-remove-chapters");
    private Option<bool> forceKeyframesAtCuts = new("--force-keyframes-at-cuts");
    private Option<bool> noForceKeyframesAtCuts = new("--no-force-keyframes-at-cuts");
    private MultiOption<string> usePostprocessor = new("--use-postprocessor");

    public bool ExtractAudio { get => extractAudio.Value; set => extractAudio.Value = value; }
    public AudioConversionFormat AudioFormat { get => audioFormat.Value; set => audioFormat.Value = value; }
    public byte? AudioQuality { get => audioQuality.Value; set => audioQuality.Value = value; }
    public string RemuxVideo { get => remuxVideo.Value; set => remuxVideo.Value = value; }
    public VideoRecodeFormat RecodeVideo { get => recodeVideo.Value; set => recodeVideo.Value = value; }
    public MultiValue<string> PostprocessorArgs { get => postprocessorArgs.Value; set => postprocessorArgs.Value = value; }
    public bool KeepVideo { get => keepVideo.Value; set => keepVideo.Value = value; }
    public bool NoKeepVideo { get => noKeepVideo.Value; set => noKeepVideo.Value = value; }
    public bool PostOverwrites { get => postOverwrites.Value; set => postOverwrites.Value = value; }
    public bool NoPostOverwrites { get => noPostOverwrites.Value; set => noPostOverwrites.Value = value; }
    public bool EmbedSubs { get => embedSubs.Value; set => embedSubs.Value = value; }
    public bool NoEmbedSubs { get => noEmbedSubs.Value; set => noEmbedSubs.Value = value; }
    public bool EmbedThumbnail { get => embedThumbnail.Value; set => embedThumbnail.Value = value; }
    public bool NoEmbedThumbnail { get => noEmbedThumbnail.Value; set => noEmbedThumbnail.Value = value; }
    public bool EmbedMetadata { get => embedMetadata.Value; set => embedMetadata.Value = value; }
    public bool NoEmbedMetadata { get => noEmbedMetadata.Value; set => noEmbedMetadata.Value = value; }
    public bool EmbedChapters { get => embedChapters.Value; set => embedChapters.Value = value; }
    public bool NoEmbedChapters { get => noEmbedChapters.Value; set => noEmbedChapters.Value = value; }
    public bool EmbedInfoJson { get => embedInfoJson.Value; set => embedInfoJson.Value = value; }
    public bool NoEmbedInfoJson { get => noEmbedInfoJson.Value; set => noEmbedInfoJson.Value = value; }
    public string ParseMetadata { get => parseMetadata.Value; set => parseMetadata.Value = value; }
    public MultiValue<string> ReplaceInMetadata { get => replaceInMetadata.Value; set => replaceInMetadata.Value = value; }
    public bool Xattrs { get => xattrs.Value; set => xattrs.Value = value; }
    public string ConcatPlaylist { get => concatPlaylist.Value; set => concatPlaylist.Value = value; }
    public string Fixup { get => fixup.Value; set => fixup.Value = value; }
    public string FfmpegLocation { get => ffmpegLocation.Value; set => ffmpegLocation.Value = value; }
    public MultiValue<string> Exec { get => exec.Value; set => exec.Value = value; }
    public bool NoExec { get => noExec.Value; set => noExec.Value = value; }
    public string ConvertSubs { get => convertSubs.Value; set => convertSubs.Value = value; }
    public string ConvertThumbnails { get => convertThumbnails.Value; set => convertThumbnails.Value = value; }
    public bool SplitChapters { get => splitChapters.Value; set => splitChapters.Value = value; }
    public bool NoSplitChapters { get => noSplitChapters.Value; set => noSplitChapters.Value = value; }
    public MultiValue<string> RemoveChapters { get => removeChapters.Value; set => removeChapters.Value = value; }
    public bool NoRemoveChapters { get => noRemoveChapters.Value; set => noRemoveChapters.Value = value; }
    public bool ForceKeyframesAtCuts { get => forceKeyframesAtCuts.Value; set => forceKeyframesAtCuts.Value = value; }
    public bool NoForceKeyframesAtCuts { get => noForceKeyframesAtCuts.Value; set => noForceKeyframesAtCuts.Value = value; }
    public MultiValue<string> UsePostprocessor { get => usePostprocessor.Value; set => usePostprocessor.Value = value; }
}
