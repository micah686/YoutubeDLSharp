namespace YtDlpSharp.Models;

public sealed record RunResult<T>(bool Success, string[] ErrorOutput, T Data);
