using AutoMapper;
using MediatR;
using OnlineStore.Application.Features.Products.DTOs;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Products.Queries.GetProductsList;

public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsListQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<OnlineStore.Core.Entities.Product> products;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            products = await _productRepository.SearchAsync(request.SearchTerm);
        }
        else if (request.InStockOnly)
        {
            products = await _productRepository.GetInStockProductsAsync();
        }
        else
        {
            products = await _productRepository.GetAllAsync();
        }

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}

