using OnlineStore.Core.Enums;

namespace OnlineStore.Core.Entities;

public class Order
{
    public int Id { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public decimal TotalAmount { get; set; }
    public int? PromoCodeId { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public int ShippingAddressId { get; set; }

    // Navigation properties
    public virtual Client Client { get; set; } = null!;
    public virtual PromoCode? PromoCode { get; set; }
    public virtual Address ShippingAddress { get; set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public void CalculateTotal()
    {
        var subtotal = OrderItems.Sum(item => item.Subtotal);
        TotalAmount = subtotal;
    }
}




