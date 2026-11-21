using FluentValidation;
using Inventory.Application.Products.DTOs;

namespace Inventory.Application.Products.Validations;

public class ProductUpdateValidation : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateValidation()
    {
        RuleFor(p => p.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");
        RuleFor(p => p.Description)
            .MaximumLength(500).WithMessage("Product description cannot exceed 500 characters.");
        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");
    }
}