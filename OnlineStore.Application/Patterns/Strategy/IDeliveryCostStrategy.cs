namespace OnlineStore.Application.Patterns.Strategy;

public interface IDeliveryCostStrategy
{
    decimal CalculateDeliveryCost(decimal orderTotal);
}



