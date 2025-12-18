using MediatR;
using OnlineStore.Application.Features.Reports.DTOs;

namespace OnlineStore.Application.Features.Reports.Queries.GetRevenueReport;

public class GetRevenueReportQuery : IRequest<RevenueReportDto>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

