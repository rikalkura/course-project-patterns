using OnlineStore.Core.Entities;
using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Patterns.Observer;

public class OrderStatusSubject
{
    private readonly List<IOrderStatusObserver> _observers = new();

    public void Attach(IOrderStatusObserver observer)
    {
        if (observer == null)
            throw new ArgumentNullException(nameof(observer));

        _observers.Add(observer);
    }

    public void Detach(IOrderStatusObserver observer)
    {
        _observers.Remove(observer);
    }

    public async Task NotifyAsync(Order order, OrderStatus previousStatus, OrderStatus newStatus)
    {
        var tasks = _observers.Select(observer => observer.NotifyAsync(order, previousStatus, newStatus));
        await Task.WhenAll(tasks);
    }
}



