using AutoMapper;
using OnlineStore.Application.Features.Orders.DTOs;
using OnlineStore.Application.Features.Products.DTOs;
using OnlineStore.Application.Patterns.Strategy;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Enums;

namespace OnlineStore.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(src => src.IsInStock));

        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

        // Order mappings
        CreateMap<Core.Entities.Order, OrderDto>()
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.FullName))
            .ForMember(dest => dest.PromoCode, opt => opt.MapFrom(src => src.PromoCode != null ? src.PromoCode.Code : null))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => 
                src.PromoCode != null && src.OrderItems != null && src.OrderItems.Any()
                    ? src.OrderItems.Sum(item => item.Subtotal) * (src.PromoCode.DiscountPercentage / 100m)
                    : 0m))
            .ForMember(dest => dest.DeliveryCost, opt => opt.MapFrom<OrderDeliveryCostResolver>());

        CreateMap<Core.Entities.OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

        CreateMap<Core.Entities.Address, AddressDto>();
    }
}
