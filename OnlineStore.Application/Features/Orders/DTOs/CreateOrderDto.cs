using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Features.Orders.DTOs;

public class CreateOrderDto
{
    public int ShippingAddressId { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? PromoCode { get; set; }
}




