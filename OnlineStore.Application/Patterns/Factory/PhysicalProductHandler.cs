using OnlineStore.Core.Entities;

namespace OnlineStore.Application.Patterns.Factory;

public class PhysicalProductHandler : IProductTypeHandler
{
    private readonly Product _product;

    public PhysicalProductHandler(Product product)
    {
        _product = product;
    }

    public Task<bool> ValidateStockAsync(int requestedQuantity)
    {
        return Task.FromResult(_product.StockQuantity >= requestedQuantity);
    }

    public Task ProcessOrderAsync(int quantity)
    {
        // Physical products require inventory management
        _product.StockQuantity -= quantity;
        return Task.CompletedTask;
    }
}



