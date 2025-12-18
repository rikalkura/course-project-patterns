using MediatR;
using Microsoft.AspNetCore.Http;
using OnlineStore.Core.Interfaces;
using System.Text.Json;

namespace OnlineStore.Application.Features.Cart.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateCartItemQuantityCommandHandler(
        IProductRepository productRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        if (request.Quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.");
        }

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

        cart[request.ProductId] = request.Quantity;
        session.SetString("Cart", JsonSerializer.Serialize(cart));

        return Unit.Value;
    }
}




