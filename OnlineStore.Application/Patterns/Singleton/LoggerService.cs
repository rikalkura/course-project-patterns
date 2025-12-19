namespace OnlineStore.Application.Patterns.Singleton;

public sealed class LoggerService : ILoggerService
{
    private static LoggerService? _instance;
    private static readonly object _lock = new();

    private LoggerService()
    {
        // Private constructor to prevent instantiation
    }

    public static LoggerService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new LoggerService();
                }
            }
            return _instance;
        }
    }

    public void LogInfo(string message)
    {
        Console.WriteLine($"[INFO] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {message}");
    }

    public void LogWarning(string message)
    {
        Console.WriteLine($"[WARNING] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {message}");
    }

    public void LogError(string message, Exception? exception = null)
    {
        Console.WriteLine($"[ERROR] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {message}");
        if (exception != null)
        {
            Console.WriteLine($"Exception: {exception.Message}");
            Console.WriteLine($"Stack Trace: {exception.StackTrace}");
        }
    }
}



