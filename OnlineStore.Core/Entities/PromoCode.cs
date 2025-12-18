namespace OnlineStore.Core.Entities;

public class PromoCode
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public bool IsValid(DateTime currentDate)
    {
        return IsActive && currentDate >= ValidFrom && currentDate <= ValidTo;
    }
}


