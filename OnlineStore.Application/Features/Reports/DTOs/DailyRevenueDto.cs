namespace OnlineStore.Application.Features.Reports.DTOs;

public class DailyRevenueDto
{
    public DateTime Date { get; set; }
    public int OrderCount { get; set; }
    public decimal Revenue { get; set; }
}

