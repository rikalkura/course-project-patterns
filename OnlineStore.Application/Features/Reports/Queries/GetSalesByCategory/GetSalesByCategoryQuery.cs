using MediatR;
using OnlineStore.Application.Features.Reports.DTOs;

namespace OnlineStore.Application.Features.Reports.Queries.GetSalesByCategory;

public class GetSalesByCategoryQuery : IRequest<IEnumerable<SalesByCategoryDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}



