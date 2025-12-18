using MediatR;
using OnlineStore.Application.Features.Products.DTOs;

namespace OnlineStore.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<int>
{
    public CreateProductDto Product { get; set; } = null!;
}


