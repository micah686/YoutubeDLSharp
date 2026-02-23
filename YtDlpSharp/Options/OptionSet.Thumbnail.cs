namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<bool> writeThumbnail = new("--write-thumbnail");
    private Option<bool> noWriteThumbnail = new("--no-write-thumbnail");
    private Option<bool> writeAllThumbnails = new("--write-all-thumbnails");
    private Option<bool> listThumbnails = new("--list-thumbnails");

    public bool WriteThumbnail { get => writeThumbnail.Value; set => writeThumbnail.Value = value; }
    public bool NoWriteThumbnail { get => noWriteThumbnail.Value; set => noWriteThumbnail.Value = value; }
    public bool WriteAllThumbnails { get => writeAllThumbnails.Value; set => writeAllThumbnails.Value = value; }
    public bool ListThumbnails { get => listThumbnails.Value; set => listThumbnails.Value = value; }
}
