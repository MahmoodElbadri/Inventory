using FluentValidation;
using Inventory.Application.Products.DTOs;

namespace Inventory.Application.Products.Validations;

public class RestockAddRequestValidator : AbstractValidator<RestockAddRequest>
{
    public RestockAddRequestValidator()
    {
        RuleFor(p => p.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}