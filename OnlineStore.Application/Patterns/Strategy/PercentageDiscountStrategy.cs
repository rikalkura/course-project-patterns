namespace OnlineStore.Application.Patterns.Strategy;

public class PercentageDiscountStrategy : IPricingStrategy
{
    private readonly decimal _discountPercentage;

    public PercentageDiscountStrategy(decimal discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount percentage must be between 0 and 100.");

        _discountPercentage = discountPercentage;
    }

    public decimal CalculatePrice(decimal basePrice, decimal discountAmount)
    {
        var discount = basePrice * (_discountPercentage / 100);
        return Math.Max(0, basePrice - discount);
    }
}



