using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.Reports.Queries.GetPopularItems;
using OnlineStore.Application.Features.Reports.Queries.GetRevenueReport;
using OnlineStore.Application.Features.Reports.Queries.GetSalesByCategory;

namespace OnlineStore.Web.Controllers.Admin;

[Authorize(Roles = "Admin")]
public class AdminReportsController : Controller
{
    private readonly IMediator _mediator;

    public AdminReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: Admin/AdminReports
    public IActionResult Index()
    {
        return View();
    }

    // GET: Admin/AdminReports/SalesByCategory
    public async Task<IActionResult> SalesByCategory(DateTime? startDate, DateTime? endDate)
    {
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;

        var query = new GetSalesByCategoryQuery
        {
            StartDate = startDate,
            EndDate = endDate
        };

        var sales = await _mediator.Send(query);
        return View(sales);
    }

    // GET: Admin/AdminReports/Revenue
    public async Task<IActionResult> Revenue(DateTime? startDate, DateTime? endDate)
    {
        if (!startDate.HasValue || !endDate.HasValue)
        {
            endDate = DateTime.UtcNow;
            startDate = endDate.Value.AddDays(-30); // Default to last 30 days
        }

        var query = new GetRevenueReportQuery
        {
            StartDate = startDate.Value,
            EndDate = endDate.Value
        };

        var report = await _mediator.Send(query);
        return View(report);
    }

    // GET: Admin/AdminReports/PopularItems
    public async Task<IActionResult> PopularItems(int topN = 10, DateTime? startDate = null, DateTime? endDate = null)
    {
        ViewBag.TopN = topN;
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;

        var query = new GetPopularItemsQuery
        {
            TopN = topN,
            StartDate = startDate,
            EndDate = endDate
        };

        var items = await _mediator.Send(query);
        return View(items);
    }
}



