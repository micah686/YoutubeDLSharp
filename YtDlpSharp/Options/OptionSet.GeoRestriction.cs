namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<string> geoVerificationProxy = new("--geo-verification-proxy");
    private Option<string> xff = new("--xff");

    public string GeoVerificationProxy { get => geoVerificationProxy.Value; set => geoVerificationProxy.Value = value; }
    public string Xff { get => xff.Value; set => xff.Value = value; }
}
