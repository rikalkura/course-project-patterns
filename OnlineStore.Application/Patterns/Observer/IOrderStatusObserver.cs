using OnlineStore.Core.Entities;
using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Patterns.Observer;

public interface IOrderStatusObserver
{
    Task NotifyAsync(Order order, OrderStatus previousStatus, OrderStatus newStatus);
}



