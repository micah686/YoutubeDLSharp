namespace YtDlpSharp.Options;

public partial class OptionSet
{
    private Option<string> username = new("-u", "--username");
    private Option<string> password = new("-p", "--password");
    private Option<string> twoFactor = new("-2", "--twofactor");
    private Option<bool> netrc = new("-n", "--netrc");
    private Option<string> netrcLocation = new("--netrc-location");
    private Option<string> netrcCmd = new("--netrc-cmd");
    private Option<string> videoPassword = new("--video-password");
    private Option<string> apMso = new("--ap-mso");
    private Option<string> apUsername = new("--ap-username");
    private Option<string> apPassword = new("--ap-password");
    private Option<bool> apListMso = new("--ap-list-mso");
    private Option<string> clientCertificate = new("--client-certificate");
    private Option<string> clientCertificateKey = new("--client-certificate-key");
    private Option<string> clientCertificatePassword = new("--client-certificate-password");

    public string Username { get => username.Value; set => username.Value = value; }
    public string Password { get => password.Value; set => password.Value = value; }
    public string TwoFactor { get => twoFactor.Value; set => twoFactor.Value = value; }
    public bool Netrc { get => netrc.Value; set => netrc.Value = value; }
    public string NetrcLocation { get => netrcLocation.Value; set => netrcLocation.Value = value; }
    public string NetrcCmd { get => netrcCmd.Value; set => netrcCmd.Value = value; }
    public string VideoPassword { get => videoPassword.Value; set => videoPassword.Value = value; }
    public string ApMso { get => apMso.Value; set => apMso.Value = value; }
    public string ApUsername { get => apUsername.Value; set => apUsername.Value = value; }
    public string ApPassword { get => apPassword.Value; set => apPassword.Value = value; }
    public bool ApListMso { get => apListMso.Value; set => apListMso.Value = value; }
    public string ClientCertificate { get => clientCertificate.Value; set => clientCertificate.Value = value; }
    public string ClientCertificateKey { get => clientCertificateKey.Value; set => clientCertificateKey.Value = value; }
    public string ClientCertificatePassword { get => clientCertificatePassword.Value; set => clientCertificatePassword.Value = value; }
}
