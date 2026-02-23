using System.IO.Compression;
using System.Net.Http;
using YtDlpSharp.Helpers;

namespace YtDlpSharp;

public static class Utils
{
    private static readonly Dictionary<char, string> AccentChars =
        "ÂÃÄÀÁÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖŐØŒÙÚÛÜŰÝÞßàáâãäåæçèéêëìíîïðñòóôõöőøœùúûüűýþÿ"
            .Zip(["A","A","A","A","A","A","AE","C","E","E","E","E","I","I","I","I","D","N",
                "O","O","O","O","O","O","O","OE","U","U","U","U","U","Y","P","ss",
                "a","a","a","a","a","a","ae","c","e","e","e","e","i","i","i","i","o","n",
                "o","o","o","o","o","o","o","oe","u","u","u","u","u","y","p","y"])
            .ToDictionary(pair => pair.First, pair => pair.Second);

    public static int TimeoutSeconds { get; set; } = 300;

    public static string YtDlpBinaryName => GetYtDlpBinaryName();
    public static string FfmpegBinaryName => GetBinaryName("ffmpeg");
    public static string FfprobeBinaryName => GetBinaryName("ffprobe");

    public static string Sanitize(string s, bool restricted = false)
    {
        var result = string.Create(s.Length * 2, (s, restricted), static (span, state) =>
        {
            int pos = 0;
            foreach (char c in state.s)
            {
                var replacement = SanitizeChar(c, state.restricted);
                foreach (char rc in replacement)
                {
                    if (pos < span.Length)
                        span[pos++] = rc;
                }
            }
            span[pos..].Fill('\0');
        });

        result = result.TrimEnd('\0');
        result = result.Replace("__", "_").Trim('_');
        if (restricted && result.StartsWith("-_"))
            result = result[2..];
        if (result.StartsWith('-'))
            result = "_" + result[1..];
        result = result.TrimStart('.');
        return string.IsNullOrWhiteSpace(result) ? "_" : result;
    }

    private static string SanitizeChar(char c, bool restricted) => c switch
    {
        _ when restricted && AccentChars.TryGetValue(c, out var replacement) => replacement,
        '?' or (< (char)32) or ((char)127) => "",
        '"' => restricted ? "" : "'",
        ':' => restricted ? "_-" : " -",
        '\\' or '/' or '|' or '*' or '<' or '>' => "_",
        _ when restricted && "!&'()[]{}$;`^,# ".Contains(c) => "_",
        _ when restricted && c > 127 => "_",
        _ => c.ToString()
    };

    public static string? GetFullPath(string fileName)
    {
        if (File.Exists(fileName))
            return Path.GetFullPath(fileName);

        var pathVar = Environment.GetEnvironmentVariable("PATH");
        if (pathVar is null) return null;

        foreach (var p in pathVar.Split(Path.PathSeparator))
        {
            var fullPath = Path.Combine(p, fileName);
            if (File.Exists(fullPath))
                return fullPath;
        }
        return null;
    }

    public static async Task DownloadBinaries(bool skipExisting = true, string directoryPath = "", HttpClient? client = null)
    {
        client ??= CreateHttpClient();
        directoryPath = string.IsNullOrEmpty(directoryPath) ? Directory.GetCurrentDirectory() : directoryPath;

        if (!skipExisting || !File.Exists(Path.Combine(directoryPath, GetYtDlpBinaryName())))
            await DownloadYtDlp(directoryPath, client);

        if (!skipExisting || !File.Exists(Path.Combine(directoryPath, GetBinaryName("ffmpeg"))))
            await DownloadFFmpeg(directoryPath, client);

        if (!skipExisting || !File.Exists(Path.Combine(directoryPath, GetBinaryName("ffprobe"))))
            await DownloadFFprobe(directoryPath, client);
    }

    public static async Task DownloadYtDlp(string directoryPath = "", HttpClient? client = null)
    {
        client ??= CreateHttpClient();
        directoryPath = string.IsNullOrEmpty(directoryPath) ? Directory.GetCurrentDirectory() : directoryPath;

        var downloadUrl = GetYtDlpDownloadUrl();
        var data = await client.GetByteArrayAsync(downloadUrl);
        var downloadLocation = Path.Combine(directoryPath, Path.GetFileName(downloadUrl));
        await File.WriteAllBytesAsync(downloadLocation, data);
        SetUnixExecPerms(downloadLocation);
    }

    public static async Task DownloadFFmpeg(string directoryPath = "", HttpClient? client = null)
    {
        await DownloadFFBinary(directoryPath, "ffmpeg", client);
    }

    public static async Task DownloadFFprobe(string directoryPath = "", HttpClient? client = null)
    {
        await DownloadFFBinary(directoryPath, "ffprobe", client);
    }

    public static void EnsureSuccess<T>(this RunResult<T> runResult)
    {
        if (!runResult.Success)
            throw new InvalidOperationException($"Download failed:\n{string.Join('\n', runResult.ErrorOutput)}");
    }

    private static string GetYtDlpBinaryName() => Path.GetFileName(GetYtDlpDownloadUrl());

    private static string GetBinaryName(string baseName) => OsHelper.GetOsVersion() switch
    {
        OsVersion.Windows => $"{baseName}.exe",
        _ => baseName
    };

    private static string GetYtDlpDownloadUrl()
    {
        const string baseUrl = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp";
        return OsHelper.GetOsVersion() switch
        {
            OsVersion.Windows => $"{baseUrl}.exe",
            OsVersion.OSX => $"{baseUrl}_macos",
            OsVersion.Linux => baseUrl,
            _ => throw new PlatformNotSupportedException()
        };
    }

    private static async Task DownloadFFBinary(string directoryPath, string binary, HttpClient? client)
    {
        client ??= CreateHttpClient();
        directoryPath = string.IsNullOrEmpty(directoryPath) ? Directory.GetCurrentDirectory() : directoryPath;

        const string ffmpegApiUrl = "https://ffbinaries.com/api/v1/version/latest";
        var response = await client.GetStringAsync(ffmpegApiUrl);
        using var doc = System.Text.Json.JsonDocument.Parse(response);
        var bin = doc.RootElement.GetProperty("bin");

        var osProp = OsHelper.GetOsVersion() switch
        {
            OsVersion.Windows => "windows-64",
            OsVersion.OSX => "osx-64",
            OsVersion.Linux => "linux-64",
            _ => throw new PlatformNotSupportedException()
        };

        var osBin = bin.GetProperty(osProp);
        var downloadUrl = osBin.GetProperty(binary).GetString()!;

        var dataBytes = await client.GetByteArrayAsync(downloadUrl);
        using var stream = new MemoryStream(dataBytes);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        if (archive.Entries.Count > 0)
        {
            var fileName = Path.Combine(directoryPath, archive.Entries[0].FullName);
            archive.Entries[0].ExtractToFile(fileName, true);
            SetUnixExecPerms(fileName);
        }
    }

    private static void SetUnixExecPerms(string filename)
    {
        if (OsHelper.IsWindows) return;
#pragma warning disable CA1416
        File.SetUnixFileMode(filename, UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite);
#pragma warning restore CA1416
    }

    private static HttpClient CreateHttpClient() => new() { Timeout = TimeSpan.FromSeconds(TimeoutSeconds) };
}
