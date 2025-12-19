using MediatR;
using OnlineStore.Application.Features.Reports.DTOs;
using OnlineStore.Core.Enums;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Reports.Queries.GetPopularItems;

public class GetPopularItemsQueryHandler : IRequestHandler<GetPopularItemsQuery, IEnumerable<PopularItemDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetPopularItemsQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<PopularItemDto>> Handle(GetPopularItemsQuery request, CancellationToken cancellationToken)
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

        var popularItems = validOrders
            .SelectMany(o => o.OrderItems)
            .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
            .Select(g => new PopularItemDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                TotalQuantitySold = g.Sum(oi => oi.Quantity),
                TotalRevenue = g.Sum(oi => oi.Subtotal),
                OrderCount = g.Select(oi => oi.OrderId).Distinct().Count()
            })
            .OrderByDescending(p => p.TotalQuantitySold)
            .Take(request.TopN)
            .ToList();

        return popularItems;
    }
}

