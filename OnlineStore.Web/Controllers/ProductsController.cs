using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Application.Features.Products.Commands.CreateProduct;
using OnlineStore.Application.Features.Products.Commands.DeleteProduct;
using OnlineStore.Application.Features.Products.Commands.UpdateProduct;
using OnlineStore.Application.Features.Products.DTOs;
using OnlineStore.Application.Features.Products.Queries.GetProductById;
using OnlineStore.Application.Features.Products.Queries.GetProductsByCategory;
using OnlineStore.Application.Features.Products.Queries.GetProductsList;
using OnlineStore.Core.Interfaces;
using OnlineStore.Infrastructure.Services;
using OnlineStore.Web.Filters;

namespace OnlineStore.Web.Controllers;

[RedirectAdmin]
public class ProductsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IFileService _fileService;
    private readonly ICategoryRepository _categoryRepository;

    public ProductsController(IMediator mediator, IFileService fileService, ICategoryRepository categoryRepository)
    {
        _mediator = mediator;
        _fileService = fileService;
        _categoryRepository = categoryRepository;
    }

    // GET: Products
    public async Task<IActionResult> Index(string? searchTerm, bool inStockOnly = false, int? categoryId = null)
    {
        // Get all categories for the dropdown
        var categories = await _categoryRepository.GetAllAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
        ViewBag.SearchTerm = searchTerm;
        ViewBag.InStockOnly = inStockOnly;
        ViewBag.CategoryId = categoryId;

        var query = new GetProductsListQuery
        {
            SearchTerm = searchTerm,
            InStockOnly = inStockOnly,
            CategoryId = categoryId
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
    public async Task<IActionResult> Create(CreateProductDto productDto, IFormFile? photoFile)
    {
        if (!ModelState.IsValid)
        {
            return View(productDto);
        }

        // Handle file upload
        if (photoFile != null && _fileService.IsValidImageFile(photoFile))
        {
            productDto.PhotoPath = await _fileService.SaveProductImageAsync(photoFile);
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
    public async Task<IActionResult> Edit(int id, UpdateProductDto updateDto, IFormFile? photoFile)
    {
        if (id != updateDto.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(updateDto);
        }

        // Handle file upload - delete old file if new one is uploaded
        if (photoFile != null && _fileService.IsValidImageFile(photoFile))
        {
            // Delete old photo if exists and it's a local file (not a Cloudinary URL)
            if (!string.IsNullOrEmpty(updateDto.PhotoPath) && 
                !updateDto.PhotoPath.StartsWith("http://") && 
                !updateDto.PhotoPath.StartsWith("https://"))
            {
                await _fileService.DeleteProductImageAsync(updateDto.PhotoPath);
            }

            updateDto.PhotoPath = await _fileService.SaveProductImageAsync(photoFile);
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
        // Get product to delete photo
        var query = new GetProductByIdQuery { Id = id };
        var product = await _mediator.Send(query);

        var command = new DeleteProductCommand { Id = id };
        await _mediator.Send(command);

        // Delete associated photo file (only if it's a local file, not a Cloudinary URL)
        if (product != null && !string.IsNullOrEmpty(product.PhotoPath) &&
            !product.PhotoPath.StartsWith("http://") && 
            !product.PhotoPath.StartsWith("https://"))
        {
            await _fileService.DeleteProductImageAsync(product.PhotoPath);
        }

        return RedirectToAction(nameof(Index));
    }
}


