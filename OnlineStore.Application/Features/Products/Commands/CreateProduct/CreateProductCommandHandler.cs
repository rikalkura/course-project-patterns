using AutoMapper;
using MediatR;
using OnlineStore.Application.Features.Products.DTOs;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Validate category exists
        var category = await _categoryRepository.GetByIdAsync(request.Product.CategoryId);
        if (category == null)
        {
            throw new InvalidOperationException($"Category with ID {request.Product.CategoryId} not found.");
        }

        // Map DTO to entity
        var product = _mapper.Map<Product>(request.Product);

        // Add product
        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}


