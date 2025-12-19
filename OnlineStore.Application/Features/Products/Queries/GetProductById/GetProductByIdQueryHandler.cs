using AutoMapper;
using MediatR;
using OnlineStore.Application.Features.Products.DTOs;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdWithCategoryAsync(request.Id);
        if (product == null)
        {
            return null;
        }

        return _mapper.Map<ProductDto>(product);
    }
}




