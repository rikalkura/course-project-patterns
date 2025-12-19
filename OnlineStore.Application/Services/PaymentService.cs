using OnlineStore.Application.Patterns.Singleton;

namespace OnlineStore.Application.Services;

public interface IPaymentService
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
}

public class PaymentService : IPaymentService
{
    private readonly ILoggerService _logger;

    public PaymentService(ILoggerService logger)
    {
        _logger = logger;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        _logger.LogInfo($"Processing payment for order {request.OrderId}, Amount: {request.Amount}");

        // Simulate payment processing delay
        await Task.Delay(500);

        // Simulate payment success/failure (90% success rate for demo)
        var random = new Random();
        var isSuccess = random.Next(1, 11) <= 9;

        if (isSuccess)
        {
            _logger.LogInfo($"Payment successful for order {request.OrderId}");
            return new PaymentResult
            {
                IsSuccess = true,
                TransactionId = Guid.NewGuid().ToString(),
                Message = "Payment processed successfully"
            };
        }
        else
        {
            _logger.LogWarning($"Payment failed for order {request.OrderId}");
            return new PaymentResult
            {
                IsSuccess = false,
                TransactionId = null,
                Message = "Payment processing failed. Please try again."
            };
        }
    }
}

public class PaymentRequest
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string CardHolderName { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;
}

public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string? TransactionId { get; set; }
    public string Message { get; set; } = string.Empty;
}

