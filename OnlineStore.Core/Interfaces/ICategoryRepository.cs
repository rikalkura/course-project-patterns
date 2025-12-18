using OnlineStore.Core.Entities;

namespace OnlineStore.Core.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
}


