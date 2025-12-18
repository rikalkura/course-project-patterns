using OnlineStore.Application.Patterns.Factory;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Patterns.Observer;

public class EmailNotificationObserver : IOrderStatusObserver
{
    private readonly INotificationFactory _notificationFactory;

    public EmailNotificationObserver(INotificationFactory notificationFactory)
    {
        _notificationFactory = notificationFactory;
    }

    public async Task NotifyAsync(Order order, OrderStatus previousStatus, OrderStatus newStatus)
    {
        if (order.Client == null)
        {
            // Skip notification if client is not loaded or null
            return;
        }

        var notification = _notificationFactory.CreateNotification("email");
        var subject = $"Order #{order.Id} Status Update";
        var message = $"Your order status has been updated from {previousStatus} to {newStatus}.";

        await notification.SendAsync(order.Client.Email ?? string.Empty, subject, message);
    }
}



