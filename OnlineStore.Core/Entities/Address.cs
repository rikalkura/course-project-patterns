namespace OnlineStore.Core.Entities;

public class Address
{
    public int Id { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsDefault { get; set; }

    // Navigation properties
    public virtual Client Client { get; set; } = null!;
}


