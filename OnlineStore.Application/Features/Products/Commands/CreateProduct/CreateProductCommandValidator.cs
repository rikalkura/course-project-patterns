using FluentValidation;
using OnlineStore.Application.Features.Products.DTOs;

namespace OnlineStore.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Product.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.");

        RuleFor(x => x.Product.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.Product.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Product.CategoryId)
            .GreaterThan(0).WithMessage("Category ID must be greater than zero.");

        RuleFor(x => x.Product.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");

        RuleFor(x => x.Product.PhotoPath)
            .MaximumLength(500).WithMessage("Photo path must not exceed 500 characters.");
    }
}




