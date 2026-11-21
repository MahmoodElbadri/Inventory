using FluentValidation;
using Inventory.Application.Products.DTOs;

namespace Inventory.Application.Products.Validations;

public class ProductAdjustmentValidation : AbstractValidator<AdjustAddRequest>
{
    public ProductAdjustmentValidation()
    {
        RuleFor(p => p.NewQuantity)
            .NotEqual(0).WithMessage("Quantity shouldn't be equal to zero.");
        RuleFor(p => p.ProductId)
            .GreaterThan(0)
            .WithMessage("Product Id should be greater than 0");
        RuleFor(p => p.Reason)
            .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");
    }
}