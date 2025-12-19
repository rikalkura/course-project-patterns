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

        // Handle combinations of filters
        bool hasCategoryFilter = request.CategoryId.HasValue && request.CategoryId.Value > 0;
        bool hasSearchTerm = !string.IsNullOrWhiteSpace(request.SearchTerm);

        if (hasCategoryFilter && hasSearchTerm)
        {
            // Both category and search term: search within the category
            products = await _productRepository.SearchByCategoryAsync(request.CategoryId.Value, request.SearchTerm);
        }
        else if (hasCategoryFilter)
        {
            // Only category filter
            products = await _productRepository.GetByCategoryIdAsync(request.CategoryId.Value);
        }
        else if (hasSearchTerm)
        {
            // Only search term
            products = await _productRepository.SearchAsync(request.SearchTerm);
        }
        else if (request.InStockOnly)
        {
            // Only in-stock filter
            products = await _productRepository.GetInStockProductsAsync();
        }
        else
        {
            // No filters: get all products
            products = await _productRepository.GetAllAsync();
        }

        // Apply in-stock filter if needed (when other filters are used)
        if (request.InStockOnly && (hasCategoryFilter || hasSearchTerm))
        {
            products = products.Where(p => p.StockQuantity > 0);
        }

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}

