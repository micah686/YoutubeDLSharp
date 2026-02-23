namespace YtDlpSharp.Helpers;

public sealed class ProcessRunner
{
    private const int MaxCount = 100;
    private readonly SemaphoreSlim _semaphore;

    public int TotalCount { get; private set; }

    public ProcessRunner(int initialCount)
    {
        _semaphore = new SemaphoreSlim(initialCount, MaxCount);
        TotalCount = initialCount;
    }

    public async Task<(int ExitCode, string[] Errors)> RunThrottledAsync(
        YtDlpProcess process,
        string[] urls,
        OptionSet options,
        CancellationToken ct,
        IProgress<DownloadProgress>? progress = null)
    {
        var errors = new List<string>();
        process.ErrorReceived += (_, e) =>
        {
            if (e.Data is not null)
                errors.Add(e.Data);
        };

        await _semaphore.WaitAsync(ct);
        try
        {
            var exitCode = await process.RunAsync(urls, options, ct, progress);
            return (exitCode, errors.ToArray());
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task SetTotalCountAsync(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, MaxCount);

        if (count > TotalCount)
        {
            _semaphore.Release(count - TotalCount);
            TotalCount = count;
        }
        else if (count < TotalCount)
        {
            var decr = TotalCount - count;
            var tasks = Enumerable.Range(0, decr).Select(_ => _semaphore.WaitAsync());
            TotalCount = count;
            await Task.WhenAll(tasks);
        }
    }
}
