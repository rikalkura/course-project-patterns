using MediatR;
using OnlineStore.Application.Features.Orders.DTOs;

namespace OnlineStore.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<int>
{
    public string ClientId { get; set; } = string.Empty;
    public CreateOrderDto OrderData { get; set; } = null!;
}




