using MediatR;

namespace OnlineStore.Application.Features.Cart.Commands.AddToCart;

public class AddToCartCommand : IRequest<Unit>
{
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}




