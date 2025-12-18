using MediatR;
using OnlineStore.Application.Features.Reports.DTOs;

namespace OnlineStore.Application.Features.Reports.Queries.GetPopularItems;

public class GetPopularItemsQuery : IRequest<IEnumerable<PopularItemDto>>
{
    public int TopN { get; set; } = 10;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

