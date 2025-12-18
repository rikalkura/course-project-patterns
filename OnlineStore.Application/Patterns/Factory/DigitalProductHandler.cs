using OnlineStore.Core.Entities;

namespace OnlineStore.Application.Patterns.Factory;

public class DigitalProductHandler : IProductTypeHandler
{
    private readonly Product _product;

    public DigitalProductHandler(Product product)
    {
        _product = product;
    }

    public Task<bool> ValidateStockAsync(int requestedQuantity)
    {
        // Digital products have unlimited stock
        return Task.FromResult(true);
    }

    public Task ProcessOrderAsync(int quantity)
    {
        // Digital products don't require inventory management
        // Could trigger license generation, download link, etc.
        Console.WriteLine($"Processing digital product order: {_product.Name}, Quantity: {quantity}");
        return Task.CompletedTask;
    }
}



