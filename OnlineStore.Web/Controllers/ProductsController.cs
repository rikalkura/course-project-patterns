using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Features.Products.Commands.CreateProduct;
using OnlineStore.Application.Features.Products.Commands.DeleteProduct;
using OnlineStore.Application.Features.Products.Commands.UpdateProduct;
using OnlineStore.Application.Features.Products.DTOs;
using OnlineStore.Application.Features.Products.Queries.GetProductById;
using OnlineStore.Application.Features.Products.Queries.GetProductsByCategory;
using OnlineStore.Application.Features.Products.Queries.GetProductsList;

namespace OnlineStore.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: Products
    public async Task<IActionResult> Index(string? searchTerm, bool inStockOnly = false)
    {
        var query = new GetProductsListQuery
        {
            SearchTerm = searchTerm,
            InStockOnly = inStockOnly
        };

        var products = await _mediator.Send(query);
        return View(products);
    }

    // GET: Products/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var query = new GetProductByIdQuery { Id = id };
        var product = await _mediator.Send(query);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Products/Category/5
    public async Task<IActionResult> Category(int id)
    {
        var query = new GetProductsByCategoryQuery { CategoryId = id };
        var products = await _mediator.Send(query);
        return View(products);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        return View(new CreateProductDto());
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return View(productDto);
        }

        var command = new CreateProductCommand { Product = productDto };
        var productId = await _mediator.Send(command);

        return RedirectToAction(nameof(Details), new { id = productId });
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var query = new GetProductByIdQuery { Id = id };
        var product = await _mediator.Send(query);

        if (product == null)
        {
            return NotFound();
        }

        var updateDto = new UpdateProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            PhotoPath = product.PhotoPath,
            Price = product.Price,
            CategoryId = product.CategoryId,
            StockQuantity = product.StockQuantity
        };

        return View(updateDto);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateProductDto updateDto)
    {
        if (id != updateDto.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(updateDto);
        }

        var command = new UpdateProductCommand
        {
            Id = updateDto.Id,
            Name = updateDto.Name,
            Description = updateDto.Description,
            PhotoPath = updateDto.PhotoPath,
            Price = updateDto.Price,
            CategoryId = updateDto.CategoryId,
            StockQuantity = updateDto.StockQuantity
        };

        await _mediator.Send(command);

        return RedirectToAction(nameof(Details), new { id });
    }

    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var query = new GetProductByIdQuery { Id = id };
        var product = await _mediator.Send(query);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var command = new DeleteProductCommand { Id = id };
        await _mediator.Send(command);

        return RedirectToAction(nameof(Index));
    }
}


