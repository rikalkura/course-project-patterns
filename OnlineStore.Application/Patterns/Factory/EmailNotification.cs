namespace OnlineStore.Application.Patterns.Factory;

public class EmailNotification : INotification
{
    public Task SendAsync(string recipient, string subject, string message)
    {
        // Simulate email sending
        Console.WriteLine($"[EMAIL] To: {recipient}, Subject: {subject}, Message: {message}");
        return Task.CompletedTask;
    }
}



