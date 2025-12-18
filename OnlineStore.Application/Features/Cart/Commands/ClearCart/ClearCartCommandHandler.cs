using MediatR;
using Microsoft.AspNetCore.Http;

namespace OnlineStore.Application.Features.Cart.Commands.ClearCart;

public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Unit>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClearCartCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<Unit> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.Remove("Cart");

        return Task.FromResult(Unit.Value);
    }
}


