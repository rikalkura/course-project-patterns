namespace OnlineStore.Application.Patterns.Strategy;

public class ExpressDeliveryStrategy : IDeliveryCostStrategy
{
    private readonly decimal _baseCost;
    private readonly decimal _percentageOfOrder;

    public ExpressDeliveryStrategy(decimal baseCost = 10.00m, decimal percentageOfOrder = 0.10m)
    {
        _baseCost = baseCost;
        _percentageOfOrder = percentageOfOrder;
    }

    public decimal CalculateDeliveryCost(decimal orderTotal)
    {
        var percentageCost = orderTotal * _percentageOfOrder;
        return _baseCost + percentageCost;
    }
}



