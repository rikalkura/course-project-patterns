using MediatR;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Orders.Commands.ApplyPromoCode;

public class ApplyPromoCodeCommandHandler : IRequestHandler<ApplyPromoCodeCommand, decimal>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPromoCodeRepository _promoCodeRepository;

    public ApplyPromoCodeCommandHandler(
        IOrderRepository orderRepository,
        IPromoCodeRepository promoCodeRepository)
    {
        _orderRepository = orderRepository;
        _promoCodeRepository = promoCodeRepository;
    }

    public async Task<decimal> Handle(ApplyPromoCodeCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");
        }

        if (order.Status != Core.Enums.OrderStatus.Created)
        {
            throw new InvalidOperationException("Promo code can only be applied to orders in Created status.");
        }

        var promoCode = await _promoCodeRepository.GetByCodeAsync(request.PromoCode);
        if (promoCode == null || !promoCode.IsValid(DateTime.UtcNow))
        {
            throw new InvalidOperationException("Invalid or expired promo code.");
        }

        order.PromoCodeId = promoCode.Id;
        order.PromoCode = promoCode;

        // Recalculate total with discount
        order.CalculateTotal();
        var discount = order.TotalAmount * (promoCode.DiscountPercentage / 100);
        order.TotalAmount -= discount;

        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        return order.TotalAmount;
    }
}



