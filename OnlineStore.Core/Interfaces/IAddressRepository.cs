using OnlineStore.Core.Entities;

namespace OnlineStore.Core.Interfaces;

public interface IAddressRepository : IRepository<Address>
{
    Task<IEnumerable<Address>> GetByClientIdAsync(string clientId);
    Task<Address?> GetDefaultAddressAsync(string clientId);
}


