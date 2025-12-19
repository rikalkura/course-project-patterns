using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.Cart.Commands.AddToCart;
using OnlineStore.Application.Features.Cart.Commands.ClearCart;
using OnlineStore.Application.Features.Cart.Commands.RemoveFromCart;
using OnlineStore.Application.Features.Cart.Commands.UpdateCartItemQuantity;
using OnlineStore.Application.Features.Cart.Queries.GetCart;
using OnlineStore.Web.Filters;

namespace OnlineStore.Web.Controllers;

[RedirectAdmin]
public class CartController : Controller
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: Cart
    public async Task<IActionResult> Index()
    {
        var query = new GetCartQuery();
        var cart = await _mediator.Send(query);
        return View(cart);
    }

    // POST: Cart/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var command = new AddToCartCommand
        {
            ProductId = productId,
            Quantity = quantity
        };

        await _mediator.Send(command);

        return RedirectToAction(nameof(Index));
    }

    // POST: Cart/Remove
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int productId)
    {
        var command = new RemoveFromCartCommand
        {
            ProductId = productId
        };

        await _mediator.Send(command);

        return RedirectToAction(nameof(Index));
    }

    // POST: Cart/UpdateQuantity
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
    {
        var command = new UpdateCartItemQuantityCommand
        {
            ProductId = productId,
            Quantity = quantity
        };

        await _mediator.Send(command);

        return RedirectToAction(nameof(Index));
    }

    // POST: Cart/Clear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Clear()
    {
        var command = new ClearCartCommand();
        await _mediator.Send(command);

        return RedirectToAction(nameof(Index));
    }
}



