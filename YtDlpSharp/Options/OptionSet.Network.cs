namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<string> proxy = new("--proxy");
    private Option<int?> socketTimeout = new("--socket-timeout");
    private Option<string> sourceAddress = new("--source-address");
    private Option<string> impersonate = new("--impersonate");
    private Option<bool> listImpersonateTargets = new("--list-impersonate-targets");
    private Option<bool> forceIPv4 = new("-4", "--force-ipv4");
    private Option<bool> forceIPv6 = new("-6", "--force-ipv6");
    private Option<bool> enableFileUrls = new("--enable-file-urls");

    public string Proxy { get => proxy.Value; set => proxy.Value = value; }
    public int? SocketTimeout { get => socketTimeout.Value; set => socketTimeout.Value = value; }
    public string SourceAddress { get => sourceAddress.Value; set => sourceAddress.Value = value; }
    public string Impersonate { get => impersonate.Value; set => impersonate.Value = value; }
    public bool ListImpersonateTargets { get => listImpersonateTargets.Value; set => listImpersonateTargets.Value = value; }
    public bool ForceIPv4 { get => forceIPv4.Value; set => forceIPv4.Value = value; }
    public bool ForceIPv6 { get => forceIPv6.Value; set => forceIPv6.Value = value; }
    public bool EnableFileUrls { get => enableFileUrls.Value; set => enableFileUrls.Value = value; }
}
