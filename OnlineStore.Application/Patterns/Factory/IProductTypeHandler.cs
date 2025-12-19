namespace OnlineStore.Application.Patterns.Factory;

public interface IProductTypeHandler
{
    Task<bool> ValidateStockAsync(int requestedQuantity);
    Task ProcessOrderAsync(int quantity);
}



