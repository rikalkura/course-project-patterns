using MediatR;
using OnlineStore.Application.Patterns.Observer;
using OnlineStore.Core.Enums;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly OrderStatusSubject _statusSubject;

    public UpdateOrderStatusCommandHandler(
        IOrderRepository orderRepository,
        OrderStatusSubject statusSubject)
    {
        _orderRepository = orderRepository;
        _statusSubject = statusSubject;
    }

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");
        }

        // Validate status transition
        if (!IsValidStatusTransition(order.Status, request.Status))
        {
            throw new InvalidOperationException($"Cannot transition from {order.Status} to {request.Status}.");
        }

        var previousStatus = order.Status;
        order.Status = request.Status;
        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        // Notify observers using Observer pattern
        await _statusSubject.NotifyAsync(order, previousStatus, request.Status);

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

