namespace OnlineStore.Application.Patterns.Strategy;

public class FixedAmountDiscountStrategy : IPricingStrategy
{
    public decimal CalculatePrice(decimal basePrice, decimal discountAmount)
    {
        return Math.Max(0, basePrice - discountAmount);
    }
}



