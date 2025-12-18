namespace OnlineStore.Application.Features.Reports.DTOs;

public class RevenueReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public List<DailyRevenueDto> DailyRevenues { get; set; } = new();
}



