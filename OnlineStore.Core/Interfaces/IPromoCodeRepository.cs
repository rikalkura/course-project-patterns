using OnlineStore.Core.Entities;

namespace OnlineStore.Core.Interfaces;

public interface IPromoCodeRepository : IRepository<PromoCode>
{
    Task<PromoCode?> GetByCodeAsync(string code);
    Task<IEnumerable<PromoCode>> GetActivePromoCodesAsync();
}


