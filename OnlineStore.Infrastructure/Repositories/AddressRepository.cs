using Microsoft.EntityFrameworkCore;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Interfaces;
using OnlineStore.Infrastructure.Data;

namespace OnlineStore.Infrastructure.Repositories;

public class AddressRepository : Repository<Address>, IAddressRepository
{
    public AddressRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Address>> GetByClientIdAsync(string clientId)
    {
        return await _dbSet
            .Where(a => a.ClientId == clientId)
            .OrderByDescending(a => a.IsDefault)
            .ToListAsync();
    }

    public async Task<Address?> GetDefaultAddressAsync(string clientId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(a => a.ClientId == clientId && a.IsDefault);
    }
}


