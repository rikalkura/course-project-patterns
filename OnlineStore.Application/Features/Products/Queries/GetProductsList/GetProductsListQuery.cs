using MediatR;
using OnlineStore.Application.Features.Products.DTOs;

namespace OnlineStore.Application.Features.Products.Queries.GetProductsList;

public class GetProductsListQuery : IRequest<IEnumerable<ProductDto>>
{
    public bool InStockOnly { get; set; } = false;
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
}




