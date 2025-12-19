namespace OnlineStore.Application.Patterns.Factory;

public interface INotificationFactory
{
    INotification CreateNotification(string type);
}



