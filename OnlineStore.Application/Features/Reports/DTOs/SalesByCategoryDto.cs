namespace OnlineStore.Application.Features.Reports.DTOs;

public class SalesByCategoryDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public int TotalItemsSold { get; set; }
    public decimal TotalRevenue { get; set; }
}



