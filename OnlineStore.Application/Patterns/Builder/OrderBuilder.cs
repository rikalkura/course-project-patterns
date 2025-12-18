using OnlineStore.Core.Entities;
using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Patterns.Builder;

public class OrderBuilder
{
    private Order _order = new();

    public OrderBuilder WithClient(string clientId)
    {
        _order.ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
        return this;
    }

    public OrderBuilder WithItems(ICollection<OrderItem> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("Order must have at least one item.", nameof(items));

        _order.OrderItems = items;
        return this;
    }

    public OrderBuilder WithDeliveryMethod(DeliveryMethod method)
    {
        _order.DeliveryMethod = method;
        return this;
    }

    public OrderBuilder WithPaymentMethod(PaymentMethod method)
    {
        _order.PaymentMethod = method;
        return this;
    }

    public OrderBuilder WithAddress(Address address)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address));

        _order.ShippingAddressId = address.Id;
        _order.ShippingAddress = address;
        return this;
    }

    public OrderBuilder WithPromoCode(PromoCode? promoCode)
    {
        if (promoCode != null)
        {
            _order.PromoCodeId = promoCode.Id;
            _order.PromoCode = promoCode;
        }
        return this;
    }

    public OrderBuilder WithStatus(OrderStatus status)
    {
        _order.Status = status;
        return this;
    }

    public Order Build()
    {
        // Validate required fields
        if (string.IsNullOrEmpty(_order.ClientId))
            throw new InvalidOperationException("Client ID is required.");

        if (_order.OrderItems == null || _order.OrderItems.Count == 0)
            throw new InvalidOperationException("Order must have at least one item.");

        if (_order.ShippingAddressId == 0)
            throw new InvalidOperationException("Shipping address is required.");

        // Set order date
        _order.OrderDate = DateTime.UtcNow;

        // Calculate total
        _order.CalculateTotal();

        // Apply promo code discount if applicable
        if (_order.PromoCode != null && _order.PromoCode.IsValid(DateTime.UtcNow))
        {
            var discount = _order.TotalAmount * (_order.PromoCode.DiscountPercentage / 100);
            _order.TotalAmount -= discount;
        }

        return _order;
    }

    public void Reset()
    {
        _order = new Order();
    }
}


