using MediatR;
using Microsoft.AspNetCore.Http;
using OnlineStore.Application.Features.Cart.Queries.GetCart;
using OnlineStore.Application.Patterns.Builder;
using OnlineStore.Application.Patterns.Factory;
using OnlineStore.Application.Patterns.Strategy;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Enums;
using OnlineStore.Core.Interfaces;
using System.Text.Json;

namespace OnlineStore.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IPromoCodeRepository _promoCodeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;
    private readonly IProductTypeFactory _productTypeFactory;
    private readonly IDeliveryCostStrategy _deliveryStrategy;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IAddressRepository addressRepository,
        IPromoCodeRepository promoCodeRepository,
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator,
        IProductTypeFactory productTypeFactory,
        IDeliveryCostStrategy deliveryStrategy)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _addressRepository = addressRepository;
        _promoCodeRepository = promoCodeRepository;
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
        _productTypeFactory = productTypeFactory;
        _deliveryStrategy = deliveryStrategy;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Get cart items
        var cartQuery = new GetCartQuery();
        var cart = await _mediator.Send(cartQuery, cancellationToken);

        if (cart.Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot create order with empty cart.");
        }

        // Get shipping address
        var address = await _addressRepository.GetByIdAsync(request.OrderData.ShippingAddressId);
        if (address == null || address.ClientId != request.ClientId)
        {
            throw new KeyNotFoundException("Shipping address not found or does not belong to client.");
        }

        // Get promo code if provided
        PromoCode? promoCode = null;
        if (!string.IsNullOrWhiteSpace(request.OrderData.PromoCode))
        {
            promoCode = await _promoCodeRepository.GetByCodeAsync(request.OrderData.PromoCode);
            if (promoCode != null && !promoCode.IsValid(DateTime.UtcNow))
            {
                promoCode = null; // Invalid promo code, ignore it
            }
        }

        // Build order items from cart using Factory pattern for product type handling
        var orderItems = new List<OrderItem>();
        foreach (var cartItem in cart.Items)
        {
            var product = await _productRepository.GetByIdWithCategoryAsync(cartItem.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product {cartItem.ProductId} not found.");
            }

            // Use Factory pattern to get appropriate product handler
            var productHandler = _productTypeFactory.CreateHandler(product);
            
            // Validate stock using product type handler
            var isValidStock = await productHandler.ValidateStockAsync(cartItem.Quantity);
            if (!isValidStock)
            {
                throw new InvalidOperationException($"Product {cartItem.ProductId} is not available in requested quantity.");
            }

            var orderItem = new OrderItem
            {
                ProductId = cartItem.ProductId,
                Product = product,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice
            };
            orderItem.CalculateSubtotal();

            orderItems.Add(orderItem);

            // Process order using product type handler
            await productHandler.ProcessOrderAsync(cartItem.Quantity);
            await _productRepository.UpdateAsync(product);
        }

        // Build order using Builder pattern with Strategy pattern for delivery
        var orderBuilder = new OrderBuilder();
        var order = orderBuilder
            .WithClient(request.ClientId)
            .WithItems(orderItems)
            .WithDeliveryMethod(request.OrderData.DeliveryMethod, _deliveryStrategy)
            .WithPaymentMethod(request.OrderData.PaymentMethod)
            .WithAddress(address)
            .WithPromoCode(promoCode)
            .WithStatus(OrderStatus.Created)
            .Build();

        // Save order
        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        // Clear cart
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.Remove("Cart");

        return order.Id;
    }
}


