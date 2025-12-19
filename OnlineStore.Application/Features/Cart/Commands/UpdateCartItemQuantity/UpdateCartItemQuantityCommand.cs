using MediatR;

namespace OnlineStore.Application.Features.Cart.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommand : IRequest<Unit>
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}




