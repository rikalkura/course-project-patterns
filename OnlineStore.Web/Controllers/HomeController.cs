using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.Products.Queries.GetProductsList;
using OnlineStore.Web.Filters;
using OnlineStore.Web.Models;

namespace OnlineStore.Web.Controllers;

[RedirectAdmin]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;

    public HomeController(ILogger<HomeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        // Get featured products (first 3 products)
        var query = new GetProductsListQuery
        {
            InStockOnly = true
        };
        var products = await _mediator.Send(query);
        var featuredProducts = products.Take(3).ToList();
        
        return View(featuredProducts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
