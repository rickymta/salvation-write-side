using Serilog;
using System.Text;
using System.Text.Json;

namespace Salvation.WriteSide.Commons;

public class LogProvider
{
    private const string PatternLog = "{message} {data}";
    public static void Custom(string key, string message, object data, string? fileName = null)
    {
        var logger = SetLoggerConfig(key, fileName!);
        logger.Information(PatternLog, message, data);
        logger.Dispose();
    }
    public static void Custom(string key, object data, bool json = false, string? fileName = null)
    {
        var logger = SetLoggerConfig(key, fileName!);

        if (json)
        {
            data = JsonSerializer.Serialize(data);
        }

        logger.Information("{message}", data);
        logger.Dispose();
    }
    public static void Error(string key, object data, bool json = false)
    {
        var logger = SetLoggerConfig($"error/{key}");

        if (json)
        {
            data = JsonSerializer.Serialize(data);
        }

        logger.Error("{message}", data);
        logger.Dispose();
    }
    public static void Error(Exception ex, string? message = null)
    {
        if (message != null)
        {
            Log.Error(message);
        }

        Log.Error(ex.Message);

        if (ex.StackTrace != null)
        {
            Log.Error(ex.StackTrace);
        }
    }
    public static void ErrorWithKey(string key, Exception ex)
    {
        var logger = SetLoggerConfig($"error/{key}");
        logger.Error(ex.Message);

        if (ex.StackTrace != null)
        {
            logger.Error(ex.StackTrace);
        }

        logger.Dispose();
    }
    static Serilog.Core.Logger SetLoggerConfig(string key, string? fileName = null)
    {
        var expr = "key = '" + key + "'";
        string nameAdd = fileName ?? "log_";
        string fileNameLog = $"log/{key}/{nameAdd}.txt";
        string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Message:lj} {NewLine}";
        var logger = new LoggerConfiguration()
            .Enrich.WithProperty("key", key)
            .Filter.ByIncludingOnly(expr)
            .WriteTo.Async(a => a.File(fileNameLog, rollingInterval: RollingInterval.Day, shared: true, outputTemplate: outputTemplate,
                                encoding: Encoding.UTF8, rollOnFileSizeLimit: true, fileSizeLimitBytes: 200 * 1024 * 1024))
            .CreateLogger();

        return logger;
    }
}
