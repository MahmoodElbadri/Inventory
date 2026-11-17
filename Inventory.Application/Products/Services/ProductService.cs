using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using Inventory.Application.Common;
using Inventory.Application.Exceptions;
using Inventory.Application.Products.DTOs;
using Inventory.Application.Products.Interfaces;
using InventoryApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.Application.Products.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper, IValidator<ProductAddRequest> addValidator,
    IValidator<ProductUpdateRequest> updateValidator,
    IValidator<RestockAddRequest> restockValidator) : IProductService
{
    public async Task<ProductResponse> AddProductAsync(ProductAddRequest product, CancellationToken cancellationToken)
    {
        var validationResult =
            await addValidator.ValidateAsync(product, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(
                string.Join(" ", validationResult.Errors
                    .Select(error => error.ErrorMessage)));
        }

        var isNameExist = await productRepository.IsProductNameExist(product.ProductName, cancellationToken);
        if (isNameExist)
        {
            throw new DuplicateProductNameException(product.ProductName);
        }
        var productEntity = mapper.Map<Product>(product);
        var addedProduct = await productRepository.AddProductAsync(productEntity, cancellationToken);
        return mapper.Map<ProductResponse>(addedProduct);
    }

    public async Task<bool> DeleteProductAsync(int productId, CancellationToken cancellationToken)
    {
        var isDeleted = await productRepository.DeleteProductAsync(productId, cancellationToken);
        if(!isDeleted)
        {
            throw new NotFoundException($"Product with ID {productId} not found.");
        }
        return true;
    }

    public async Task<bool> RestockProduct(RestockAddRequest restock, CancellationToken cancellationToken)
    {
        var validationResult = await restockValidator.ValidateAsync(restock, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new BadRequestException(
                string.Join(" ", validationResult.Errors
                    .Select(error => error.ErrorMessage)));
        }

        return await productRepository.RestockProduct(restock.ProductId, restock.Quantity, cancellationToken);
    }

    public async Task<PagedResult<ProductResponse>> GetAllProductsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        if(pageNumber > 1000 || pageSize > 100)
        {
            throw new BadRequestException("Page number or page size is out of range.");
        }
    
        var products = await productRepository.GetAllProductsAsync(pageNumber, pageSize, cancellationToken);
        return new PagedResult<ProductResponse>(
            mapper.Map<List<ProductResponse>>(products.Items),
            products.TotalCount,
            products.PageNumber,
            products.PageSize);
    }

    public async Task<ProductResponse?> GetProductByIdAsync(int productId, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if(product == null)
        {
            throw new NotFoundException($"Product with ID {productId} not found.");
        }
        return mapper.Map<ProductResponse?>(product);
    }

    public async Task<bool> UpdateProductAsync(ProductUpdateRequest product, CancellationToken cancellationToken)
    {
        var validationResult =
            await updateValidator.ValidateAsync(product, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(
                string.Join(" ", validationResult.Errors
                    .Select(error => error.ErrorMessage)));
        }


        var productEntity = mapper.Map<Product>(product);
        var isUpdated = await productRepository.UpdateProductAsync(product.Id, productEntity, cancellationToken);
        if(!isUpdated)
        {
            throw new NotFoundException($"Product with ID {product.Id} not found.");
        }
        return true;
    }

}
