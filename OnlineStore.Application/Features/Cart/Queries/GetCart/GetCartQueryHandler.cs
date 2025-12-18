using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using OnlineStore.Application.Features.Cart.DTOs;
using OnlineStore.Core.Interfaces;
using System.Text.Json;

namespace OnlineStore.Application.Features.Cart.Queries.GetCart;

public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public GetCartQueryHandler(
        IProductRepository productRepository,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<CartDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var cartJson = session?.GetString("Cart");

        if (string.IsNullOrEmpty(cartJson))
        {
            return new CartDto();
        }

        var cartDict = JsonSerializer.Deserialize<Dictionary<int, int>>(cartJson);
        if (cartDict == null || cartDict.Count == 0)
        {
            return new CartDto();
        }

        var cartDto = new CartDto();

        foreach (var item in cartDict)
        {
            var product = await _productRepository.GetByIdAsync(item.Key);
            if (product == null) continue;

            var cartItem = new CartItemDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductPhotoPath = product.PhotoPath,
                UnitPrice = product.Price,
                Quantity = item.Value,
                Subtotal = product.Price * item.Value
            };

            cartDto.Items.Add(cartItem);
            cartDto.TotalAmount += cartItem.Subtotal;
            cartDto.TotalItems += cartItem.Quantity;
        }

        return cartDto;
    }
}


