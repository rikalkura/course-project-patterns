using OnlineStore.Application.Patterns.Factory;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Patterns.Observer;

public class SmsNotificationObserver : IOrderStatusObserver
{
    private readonly INotificationFactory _notificationFactory;

    public SmsNotificationObserver(INotificationFactory notificationFactory)
    {
        _notificationFactory = notificationFactory;
    }

    public async Task NotifyAsync(Order order, OrderStatus previousStatus, OrderStatus newStatus)
    {
        // Only send SMS for critical status changes
        if (newStatus == OrderStatus.Shipped || newStatus == OrderStatus.Delivered)
        {
            if (order.Client == null)
            {
                // Skip notification if client is not loaded or null
                return;
            }

            var notification = _notificationFactory.CreateNotification("sms");
            var message = $"Order #{order.Id} status: {newStatus}";

            await notification.SendAsync(order.Client.PhoneNumber ?? string.Empty, string.Empty, message);
        }
    }
}



