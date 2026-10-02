using Serilog;

namespace PooSee.Services;

public sealed class LogService : ILogService, IDisposable
{
    public LogService(string logFilePath)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(logFilePath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 14,
                shared: true)
            .CreateLogger();
    }

    public void Info(string message) => Log.Information(message);
    public void Warn(string message) => Log.Warning(message);
    public void Error(string message, Exception? ex = null) => Log.Error(ex, message);
    public void Debug(string message) => Log.Debug(message);

    public void Dispose() => Log.CloseAndFlush();
}