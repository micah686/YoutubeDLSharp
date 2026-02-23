using Microsoft.Extensions.DependencyInjection;
using YtDlpSharp.Helpers;

namespace YtDlpSharp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYtDlpSharp(
        this IServiceCollection services,
        Action<YtDlpConfiguration> configure)
    {
        var config = new YtDlpConfiguration { YtDlpPath = "yt-dlp" };
        configure(config);

        services.AddSingleton(config);
        services.AddSingleton<ProcessRunner>(_ => new ProcessRunner(config.MaxConcurrentProcesses));
        services.AddTransient<IYtDlpProcess, YtDlpProcess>(_ => new YtDlpProcess(config.YtDlpPath));
        services.AddSingleton<IYtDlp, YtDlp>(sp => new YtDlp(config));

        return services;
    }
}
