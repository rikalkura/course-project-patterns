namespace OnlineStore.Application.Patterns.Factory;

public class NotificationFactory : INotificationFactory
{
    public INotification CreateNotification(string type)
    {
        return type.ToLower() switch
        {
            "email" => new EmailNotification(),
            "sms" => new SmsNotification(),
            _ => throw new ArgumentException($"Unknown notification type: {type}")
        };
    }
}



