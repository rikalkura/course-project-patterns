using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace OnlineStore.Application.Features.Cart.Commands.RemoveFromCart;

public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, Unit>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RemoveFromCartCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<Unit> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null)
        {
            throw new InvalidOperationException("Session is not available.");
        }

        var cartJson = session.GetString("Cart");
        if (string.IsNullOrEmpty(cartJson))
        {
            return Task.FromResult(Unit.Value);
        }

        var cart = JsonSerializer.Deserialize<Dictionary<int, int>>(cartJson);
        if (cart != null && cart.ContainsKey(request.ProductId))
        {
            cart.Remove(request.ProductId);
            session.SetString("Cart", JsonSerializer.Serialize(cart));
        }

        return Task.FromResult(Unit.Value);
    }
}




