namespace OnlineStore.Application.Patterns.Strategy;

public class StandardDeliveryStrategy : IDeliveryCostStrategy
{
    private readonly decimal _baseCost;
    private readonly decimal _freeShippingThreshold;

    public StandardDeliveryStrategy(decimal baseCost = 5.00m, decimal freeShippingThreshold = 50.00m)
    {
        _baseCost = baseCost;
        _freeShippingThreshold = freeShippingThreshold;
    }

    public decimal CalculateDeliveryCost(decimal orderTotal)
    {
        if (orderTotal >= _freeShippingThreshold)
            return 0;

        return _baseCost;
    }
}



