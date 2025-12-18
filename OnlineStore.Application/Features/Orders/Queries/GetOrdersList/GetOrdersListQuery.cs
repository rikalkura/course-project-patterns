using MediatR;
using OnlineStore.Application.Features.Orders.DTOs;
using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQuery : IRequest<IEnumerable<OrderDto>>
{
    public OrderStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

