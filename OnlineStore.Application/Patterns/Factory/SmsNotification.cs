namespace OnlineStore.Application.Patterns.Factory;

public class SmsNotification : INotification
{
    public Task SendAsync(string recipient, string subject, string message)
    {
        // Simulate SMS sending
        Console.WriteLine($"[SMS] To: {recipient}, Message: {message}");
        return Task.CompletedTask;
    }
}



