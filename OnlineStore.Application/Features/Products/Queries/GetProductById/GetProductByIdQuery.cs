using MediatR;
using OnlineStore.Application.Features.Products.DTOs;

namespace OnlineStore.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ProductDto?>
{
    public int Id { get; set; }
}


