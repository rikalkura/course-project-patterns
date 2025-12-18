using OnlineStore.Core.Entities;

namespace OnlineStore.Application.Patterns.Factory;

public interface IProductTypeFactory
{
    IProductTypeHandler CreateHandler(Product product);
}



