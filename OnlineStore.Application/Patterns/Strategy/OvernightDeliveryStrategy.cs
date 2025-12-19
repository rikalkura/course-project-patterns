namespace OnlineStore.Application.Patterns.Strategy;

public class OvernightDeliveryStrategy : IDeliveryCostStrategy
{
    private readonly decimal _fixedCost;

    public OvernightDeliveryStrategy(decimal fixedCost = 25.00m)
    {
        _fixedCost = fixedCost;
    }

    public decimal CalculateDeliveryCost(decimal orderTotal)
    {
        return _fixedCost;
    }
}



