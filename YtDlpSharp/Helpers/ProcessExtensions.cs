namespace YtDlpSharp.Helpers;

internal static class ProcessExtensions
{
    public static void KillTree(this Process process)
    {
        try
        {
            process.Kill(entireProcessTree: true);
        }
        catch (InvalidOperationException) { }
    }
}
