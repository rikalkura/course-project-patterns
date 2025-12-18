using MediatR;
using OnlineStore.Application.Features.Orders.DTOs;

namespace OnlineStore.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<OrderDto?>
{
    public int Id { get; set; }
}

