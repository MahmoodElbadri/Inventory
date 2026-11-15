using FluentValidation;
using Inventory.Application.Products.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Products.Validations;

public class ProductAddValidation: AbstractValidator<ProductAddRequest>
{
    public ProductAddValidation()
    {
        RuleFor(p => p.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");
        
        RuleFor(p => p.Description)
            .MaximumLength(500).WithMessage("Product description cannot exceed 500 characters.");
        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");
        RuleFor(p => p.QuantityOnHand)
            .GreaterThanOrEqualTo(0).WithMessage("Product quantity cannot be negative.");
    }
}

