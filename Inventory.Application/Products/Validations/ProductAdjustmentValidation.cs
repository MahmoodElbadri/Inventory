using FluentValidation;
using Inventory.Application.Products.DTOs;

namespace Inventory.Application.Products.Validations;

public class ProductAdjustmentValidation : AbstractValidator<AdjustAddRequest>
{
    public ProductAdjustmentValidation()
    {
        RuleFor(p => p.NewQuantity)
            .GreaterThan(-1).WithMessage("Quantity shouldn't be negative.");
        RuleFor(p => p.ProductId)
            .GreaterThan(0)
            .WithMessage("Product Id should be greater than 0");
        RuleFor(p => p.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .NotNull().WithMessage("Reason is required.")
            .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");
    }
}