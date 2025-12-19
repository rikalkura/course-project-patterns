using MediatR;
using OnlineStore.Application.Features.Reports.DTOs;
using OnlineStore.Core.Enums;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Reports.Queries.GetRevenueReport;

public class GetRevenueReportQueryHandler : IRequestHandler<GetRevenueReportQuery, RevenueReportDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetRevenueReportQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<RevenueReportDto> Handle(GetRevenueReportQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrdersByDateRangeAsync(request.StartDate, request.EndDate);
        
        var validOrders = orders
            .Where(o => o.Status != OrderStatus.Cancelled)
            .ToList();

        var totalOrders = validOrders.Count;
        var totalRevenue = validOrders.Sum(o => o.TotalAmount);
        var averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

        var dailyRevenues = validOrders
            .GroupBy(o => o.OrderDate.Date)
            .Select(g => new DailyRevenueDto
            {
                Date = g.Key,
                OrderCount = g.Count(),
                Revenue = g.Sum(o => o.TotalAmount)
            })
            .OrderBy(d => d.Date)
            .ToList();

        return new RevenueReportDto
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalOrders = totalOrders,
            TotalRevenue = totalRevenue,
            AverageOrderValue = averageOrderValue,
            DailyRevenues = dailyRevenues
        };
    }
}

