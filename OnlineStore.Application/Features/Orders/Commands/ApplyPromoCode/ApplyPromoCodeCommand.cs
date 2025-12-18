using MediatR;

namespace OnlineStore.Application.Features.Orders.Commands.ApplyPromoCode;

public class ApplyPromoCodeCommand : IRequest<decimal>
{
    public int OrderId { get; set; }
    public string PromoCode { get; set; } = string.Empty;
}

