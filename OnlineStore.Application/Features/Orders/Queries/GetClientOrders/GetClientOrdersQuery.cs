using MediatR;
using OnlineStore.Application.Features.Orders.DTOs;

namespace OnlineStore.Application.Features.Orders.Queries.GetClientOrders;

public class GetClientOrdersQuery : IRequest<IEnumerable<OrderDto>>
{
    public string ClientId { get; set; } = string.Empty;
}

