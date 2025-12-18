using OnlineStore.Core.Entities;
using OnlineStore.Core.Enums;

namespace OnlineStore.Core.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetByClientIdAsync(string clientId);
    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
    Task<Order?> GetByIdWithItemsAsync(int id);
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
}


