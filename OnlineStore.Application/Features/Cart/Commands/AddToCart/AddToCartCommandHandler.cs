using MediatR;
using Microsoft.AspNetCore.Http;
using OnlineStore.Core.Interfaces;
using System.Text.Json;

namespace OnlineStore.Application.Features.Cart.Commands.AddToCart;

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddToCartCommandHandler(
        IProductRepository productRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");
        }

        if (product.StockQuantity < request.Quantity)
        {
            throw new InvalidOperationException($"Insufficient stock. Available: {product.StockQuantity}, Requested: {request.Quantity}");
        }

        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null)
        {
            throw new InvalidOperationException("Session is not available.");
        }

        var cartJson = session.GetString("Cart");
        var cart = string.IsNullOrEmpty(cartJson)
            ? new Dictionary<int, int>()
            : JsonSerializer.Deserialize<Dictionary<int, int>>(cartJson) ?? new Dictionary<int, int>();

        if (cart.ContainsKey(request.ProductId))
        {
            cart[request.ProductId] += request.Quantity;
        }
        else
        {
            cart[request.ProductId] = request.Quantity;
        }

        session.SetString("Cart", JsonSerializer.Serialize(cart));

        return Unit.Value;
    }
}




