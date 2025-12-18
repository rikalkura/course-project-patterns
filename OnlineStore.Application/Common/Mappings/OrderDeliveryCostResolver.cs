using AutoMapper;
using OnlineStore.Application.Features.Orders.DTOs;
using OnlineStore.Application.Patterns.Strategy;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Common.Mappings;

public class OrderDeliveryCostResolver : IValueResolver<Order, OrderDto, decimal>
{
    public decimal Resolve(Order source, OrderDto destination, decimal destMember, ResolutionContext context)
    {
        if (source.OrderItems == null || !source.OrderItems.Any())
            return 0m;

        var subtotal = source.OrderItems.Sum(item => item.Subtotal);
        // Use order date for validation, not current date, to preserve historical accuracy
        var orderDate = source.OrderDate;
        var discountAmount = source.PromoCode != null && source.PromoCode.IsValid(orderDate)
            ? subtotal * (source.PromoCode.DiscountPercentage / 100m)
            : 0m;
        var subtotalAfterDiscount = subtotal - discountAmount;

        IDeliveryCostStrategy deliveryStrategy = source.DeliveryMethod switch
        {
            DeliveryMethod.Standard => new StandardDeliveryStrategy(),
            DeliveryMethod.Express => new ExpressDeliveryStrategy(),
            DeliveryMethod.Overnight => new OvernightDeliveryStrategy(),
            _ => new StandardDeliveryStrategy()
        };

        return deliveryStrategy.CalculateDeliveryCost(subtotalAfterDiscount);
    }
}

