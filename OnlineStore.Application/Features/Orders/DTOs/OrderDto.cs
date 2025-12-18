using OnlineStore.Application.Features.Orders.DTOs;
using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Features.Orders.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal DeliveryCost { get; set; }
    public int? PromoCodeId { get; set; }
    public string? PromoCode { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
    public AddressDto ShippingAddress { get; set; } = null!;
}




