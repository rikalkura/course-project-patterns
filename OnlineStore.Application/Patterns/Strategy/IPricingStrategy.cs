namespace OnlineStore.Application.Patterns.Strategy;

public interface IPricingStrategy
{
    decimal CalculatePrice(decimal basePrice, decimal discountAmount);
}



