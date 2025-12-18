using MediatR;
using OnlineStore.Application.Services;

namespace OnlineStore.Application.Features.Orders.Commands.ProcessPayment;

public class ProcessPaymentCommand : IRequest<PaymentResult>
{
    public int OrderId { get; set; }
    public PaymentRequest PaymentRequest { get; set; } = null!;
}



