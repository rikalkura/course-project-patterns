using MediatR;
using OnlineStore.Core.Enums;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");
        }

        // Validate status transition
        if (!IsValidStatusTransition(order.Status, request.Status))
        {
            throw new InvalidOperationException($"Cannot transition from {order.Status} to {request.Status}.");
        }

        order.Status = request.Status;
        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private static bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
    {
        return currentStatus switch
        {
            OrderStatus.Created => newStatus == OrderStatus.Paid || newStatus == OrderStatus.Cancelled,
            OrderStatus.Paid => newStatus == OrderStatus.Shipped || newStatus == OrderStatus.Cancelled,
            OrderStatus.Shipped => newStatus == OrderStatus.Delivered,
            OrderStatus.Delivered => newStatus == OrderStatus.Completed,
            OrderStatus.Completed => false, // Cannot change completed orders
            OrderStatus.Cancelled => false, // Cannot change cancelled orders
            _ => false
        };
    }
}

