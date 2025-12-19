using MediatR;
using OnlineStore.Application.Features.Products.DTOs;

namespace OnlineStore.Application.Features.Products.Queries.GetProductsByCategory;

public class GetProductsByCategoryQuery : IRequest<IEnumerable<ProductDto>>
{
    public int CategoryId { get; set; }
}




