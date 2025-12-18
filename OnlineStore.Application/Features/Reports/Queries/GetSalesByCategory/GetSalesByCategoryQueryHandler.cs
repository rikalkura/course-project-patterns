using MediatR;
using OnlineStore.Application.Features.Reports.DTOs;
using OnlineStore.Core.Enums;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Reports.Queries.GetSalesByCategory;

public class GetSalesByCategoryQueryHandler : IRequestHandler<GetSalesByCategoryQuery, IEnumerable<SalesByCategoryDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetSalesByCategoryQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<SalesByCategoryDto>> Handle(GetSalesByCategoryQuery request, CancellationToken cancellationToken)
    {
        var orders = request.StartDate.HasValue && request.EndDate.HasValue
            ? await _orderRepository.GetOrdersByDateRangeAsync(request.StartDate.Value, request.EndDate.Value)
            : await _orderRepository.GetAllAsync();

        var validOrders = orders
            .Where(o => o.Status != OrderStatus.Cancelled)
            .ToList();

        if (request.StartDate.HasValue)
        {
            validOrders = validOrders.Where(o => o.OrderDate >= request.StartDate.Value).ToList();
        }

        if (request.EndDate.HasValue)
        {
            validOrders = validOrders.Where(o => o.OrderDate <= request.EndDate.Value).ToList();
        }

        var salesByCategory = validOrders
            .SelectMany(o => o.OrderItems)
            .GroupBy(oi => new { oi.Product.CategoryId, oi.Product.Category.Name })
            .Select(g => new SalesByCategoryDto
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.Name,
                TotalOrders = g.Select(oi => oi.OrderId).Distinct().Count(),
                TotalItemsSold = g.Sum(oi => oi.Quantity),
                TotalRevenue = g.Sum(oi => oi.Subtotal)
            })
            .OrderByDescending(s => s.TotalRevenue)
            .ToList();

        return salesByCategory;
    }
}

