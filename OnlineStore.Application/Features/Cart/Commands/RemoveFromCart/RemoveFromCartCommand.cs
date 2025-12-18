using MediatR;

namespace OnlineStore.Application.Features.Cart.Commands.RemoveFromCart;

public class RemoveFromCartCommand : IRequest<Unit>
{
    public int ProductId { get; set; }
}


