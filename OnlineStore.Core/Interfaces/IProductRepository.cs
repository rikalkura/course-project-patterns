using OnlineStore.Core.Entities;

namespace OnlineStore.Core.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
    Task<IEnumerable<Product>> SearchAsync(string searchTerm);
    Task<IEnumerable<Product>> SearchByCategoryAsync(int categoryId, string searchTerm);
    Task<IEnumerable<Product>> GetInStockProductsAsync();
    Task<Product?> GetByIdWithCategoryAsync(int id);
}




