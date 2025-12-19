using MediatR;
using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommand : IRequest<Unit>
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
}



