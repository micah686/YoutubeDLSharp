using System.Runtime.InteropServices;

namespace YtDlpSharp.Helpers;

internal enum OsVersion { Windows, OSX, Linux }

internal static class OsHelper
{
    internal static bool IsWindows => GetOsVersion() == OsVersion.Windows;

    internal static OsVersion GetOsVersion() => true switch
    {
        _ when RuntimeInformation.IsOSPlatform(OSPlatform.Windows) => OsVersion.Windows,
        _ when RuntimeInformation.IsOSPlatform(OSPlatform.OSX) => OsVersion.OSX,
        _ when RuntimeInformation.IsOSPlatform(OSPlatform.Linux) => OsVersion.Linux,
        _ => throw new PlatformNotSupportedException("Your OS isn't supported")
    };
}
