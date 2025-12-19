using OnlineStore.Core.Entities;

namespace OnlineStore.Application.Patterns.Factory;

public class ProductTypeFactory : IProductTypeFactory
{
    public IProductTypeHandler CreateHandler(Product product)
    {
        // Determine product type based on category or product properties
        // For this example, we'll use a simple rule: if category name contains "Digital" or "Software", it's digital
        var isDigital = product.Category?.Name?.Contains("Digital", StringComparison.OrdinalIgnoreCase) == true ||
                       product.Category?.Name?.Contains("Software", StringComparison.OrdinalIgnoreCase) == true;

        return isDigital
            ? new DigitalProductHandler(product)
            : new PhysicalProductHandler(product);
    }
}



