using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.Orders.Commands.UpdateOrderStatus;
using OnlineStore.Application.Features.Orders.Queries.GetOrderById;
using OnlineStore.Application.Features.Orders.Queries.GetOrdersList;
using OnlineStore.Core.Enums;

namespace OnlineStore.Web.Controllers.Admin;

[Authorize(Roles = "Admin")]
public class AdminOrdersController : Controller
{
    private readonly IMediator _mediator;

    public AdminOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: Admin/AdminOrders
    public async Task<IActionResult> Index(OrderStatus? status, DateTime? startDate, DateTime? endDate)
    {
        ViewBag.Status = status;
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;

        var query = new GetOrdersListQuery
        {
            Status = status,
            StartDate = startDate,
            EndDate = endDate
        };

        var orders = await _mediator.Send(query);
        return View(orders);
    }

    // GET: Admin/AdminOrders/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var query = new GetOrderByIdQuery { Id = id };
        var order = await _mediator.Send(query);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // POST: Admin/AdminOrders/UpdateStatus/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
    {
        try
        {
            var command = new UpdateOrderStatusCommand
            {
                OrderId = id,
                Status = status
            };

            await _mediator.Send(command);

            TempData["Success"] = $"Order status updated to {status} successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Failed to update order status: {ex.Message}";
        }

        return RedirectToAction(nameof(Details), new { id });
    }
}



