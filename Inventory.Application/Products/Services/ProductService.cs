using System;
using AutoMapper;
using Inventory.Application.Products.DTOs;
using Inventory.Application.Products.Interfaces;
using InventoryApi.Models;

namespace Inventory.Application.Products.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
{
    public async Task<ProductResponse> AddProductAsync(ProductAddRequest product)
    {
        var productEntity = mapper.Map<Product>(product);
        var addedProduct = await productRepository.AddProductAsync(productEntity);
        return mapper.Map<ProductResponse>(addedProduct);
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var isDeleted = await productRepository.DeleteProductAsync(productId);
        if(!isDeleted)
        {
            throw new Exception($"Product with ID {productId} not found.");
        }
        return true;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
    {
        var products = await productRepository.GetAllProductsAsync();
        return mapper.Map<IEnumerable<ProductResponse>>(products);
    }

    public async Task<ProductResponse?> GetProductByIdAsync(int productId)
    {
        var product = await productRepository.GetProductByIdAsync(productId);
        if(product == null)
        {
            throw new Exception($"Product with ID {productId} not found.");
        }
        return mapper.Map<ProductResponse?>(product);
    }

    public async Task<bool> UpdateProductAsync(ProductUpdateRequest product)
    {
        var productEntity = mapper.Map<Product>(product);
        var isUpdated = await productRepository.UpdateProductAsync(productEntity);
        if(!isUpdated)
        {
            throw new Exception($"Product with ID {product.Id} not found.");
        }
        return true;
    }
}
