using MediatR;
using OnlineStore.Application.Features.Orders.Commands.UpdateOrderStatus;
using OnlineStore.Application.Services;
using OnlineStore.Core.Enums;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Orders.Commands.ProcessPayment;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, PaymentResult>
{
    private readonly IPaymentService _paymentService;
    private readonly IOrderRepository _orderRepository;
    private readonly IMediator _mediator;

    public ProcessPaymentCommandHandler(
        IPaymentService paymentService,
        IOrderRepository orderRepository,
        IMediator mediator)
    {
        _paymentService = paymentService;
        _orderRepository = orderRepository;
        _mediator = mediator;
    }

    public async Task<PaymentResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");
        }

        if (order.Status != OrderStatus.Created)
        {
            throw new InvalidOperationException($"Order {request.OrderId} cannot be paid. Current status: {order.Status}");
        }

        // Process payment
        var paymentResult = await _paymentService.ProcessPaymentAsync(request.PaymentRequest);

        if (paymentResult.IsSuccess)
        {
            // Update order status to Paid
            var updateStatusCommand = new UpdateOrderStatusCommand
            {
                OrderId = request.OrderId,
                Status = OrderStatus.Paid
            };

            await _mediator.Send(updateStatusCommand, cancellationToken);
        }

        return paymentResult;
    }
}



