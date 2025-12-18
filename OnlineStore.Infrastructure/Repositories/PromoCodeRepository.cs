using Microsoft.EntityFrameworkCore;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Interfaces;
using OnlineStore.Infrastructure.Data;

namespace OnlineStore.Infrastructure.Repositories;

public class PromoCodeRepository : Repository<PromoCode>, IPromoCodeRepository
{
    public PromoCodeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PromoCode?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(pc => pc.Code.ToUpper() == code.ToUpper());
    }

    public async Task<IEnumerable<PromoCode>> GetActivePromoCodesAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(pc => pc.IsActive && 
                        pc.ValidFrom <= now && 
                        pc.ValidTo >= now)
            .ToListAsync();
    }
}


