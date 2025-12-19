using OnlineStore.Application.Patterns.Singleton;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Patterns.Observer;

public class LoggingObserver : IOrderStatusObserver
{
    private readonly ILoggerService _logger;

    public LoggingObserver(ILoggerService logger)
    {
        _logger = logger;
    }

    public Task NotifyAsync(Order order, OrderStatus previousStatus, OrderStatus newStatus)
    {
        _logger.LogInfo($"Order #{order.Id} status changed from {previousStatus} to {newStatus}");
        return Task.CompletedTask;
    }
}

