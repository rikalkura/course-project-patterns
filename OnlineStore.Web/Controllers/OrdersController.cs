using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Application.Features.Cart.Queries.GetCart;
using OnlineStore.Application.Features.Orders.Commands.CreateOrder;
using OnlineStore.Application.Features.Orders.Commands.ProcessPayment;
using OnlineStore.Application.Features.Orders.DTOs;
using OnlineStore.Application.Features.Orders.Queries.GetClientOrders;
using OnlineStore.Application.Features.Orders.Queries.GetOrderById;
using OnlineStore.Application.Services;
using OnlineStore.Core.Enums;
using OnlineStore.Core.Interfaces;
using OnlineStore.Web.Filters;
using System.Security.Claims;

namespace OnlineStore.Web.Controllers;

[Authorize]
[RedirectAdmin]
public class OrdersController : Controller
{
    private readonly IMediator _mediator;
    private readonly IAddressRepository _addressRepository;

    public OrdersController(IMediator mediator, IAddressRepository addressRepository)
    {
        _mediator = mediator;
        _addressRepository = addressRepository;
    }

    // GET: Orders
    public async Task<IActionResult> Index()
    {
        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(clientId))
        {
            return Unauthorized();
        }

        var query = new GetClientOrdersQuery { ClientId = clientId };
        var orders = await _mediator.Send(query);
        return View(orders);
    }

    // GET: Orders/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var query = new GetOrderByIdQuery { Id = id };
        var order = await _mediator.Send(query);

        if (order == null)
        {
            return NotFound();
        }

        // Verify order belongs to current user
        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (order.ClientId != clientId)
        {
            return Forbid();
        }

        return View(order);
    }

    // GET: Orders/Create
    public async Task<IActionResult> Create()
    {
        // Get cart to verify it's not empty
        var cartQuery = new GetCartQuery();
        var cart = await _mediator.Send(cartQuery);

        if (cart.Items.Count == 0)
        {
            TempData["Error"] = "Your cart is empty. Please add items before checkout.";
            return RedirectToAction("Index", "Cart");
        }

        // Get user addresses
        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(clientId))
        {
            return Unauthorized();
        }

        var addresses = await _addressRepository.GetByClientIdAsync(clientId);
        var addressList = addresses.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = $"{a.Street}, {a.City}, {a.State} {a.ZipCode}, {a.Country}" + (a.IsDefault ? " (Default)" : "")
        }).ToList();

        ViewBag.Addresses = addressList;
        ViewBag.Cart = cart;

        // Set default address if available
        var defaultAddress = addresses.FirstOrDefault(a => a.IsDefault) ?? addresses.FirstOrDefault();
        var orderDto = new CreateOrderDto();
        if (defaultAddress != null)
        {
            orderDto.ShippingAddressId = defaultAddress.Id;
        }

        return View(orderDto);
    }

    // POST: Orders/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderDto orderDto)
    {
        // Get cart and addresses for view
        var cartQuery = new GetCartQuery();
        var cart = await _mediator.Send(cartQuery);
        ViewBag.Cart = cart;

        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(clientId))
        {
            return Unauthorized();
        }

        var addresses = await _addressRepository.GetByClientIdAsync(clientId);
        var addressList = addresses.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = $"{a.Street}, {a.City}, {a.State} {a.ZipCode}, {a.Country}" + (a.IsDefault ? " (Default)" : "")
        }).ToList();
        ViewBag.Addresses = addressList;

        if (!ModelState.IsValid)
        {
            return View(orderDto);
        }

        var command = new CreateOrderCommand
        {
            ClientId = clientId,
            OrderData = orderDto
        };

        var orderId = await _mediator.Send(command);

        return RedirectToAction(nameof(Details), new { id = orderId });
    }

    // GET: Orders/Payment/5
    public async Task<IActionResult> Payment(int id)
    {
        var query = new GetOrderByIdQuery { Id = id };
        var order = await _mediator.Send(query);

        if (order == null)
        {
            return NotFound();
        }

        // Verify order belongs to current user
        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (order.ClientId != clientId)
        {
            return Forbid();
        }

        if (order.Status != OrderStatus.Created)
        {
            TempData["Error"] = "This order has already been processed.";
            return RedirectToAction(nameof(Details), new { id });
        }

        return View(order);
    }

    // POST: Orders/ProcessPayment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessPayment(int orderId, PaymentRequest paymentRequest)
    {
        paymentRequest.OrderId = orderId;

        var command = new ProcessPaymentCommand
        {
            OrderId = orderId,
            PaymentRequest = paymentRequest
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            TempData["Success"] = "Payment processed successfully!";
            return RedirectToAction(nameof(Details), new { id = orderId });
        }
        else
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Payment), new { id = orderId });
        }
    }
}

