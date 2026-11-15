using System;
using Inventory.Application.Products.DTOs;

namespace Inventory.Application.Products.Interfaces;

public interface IProductService
{
    Task<ProductResponse> AddProductAsync(ProductAddRequest product);
    Task<ProductResponse?> GetProductByIdAsync(int productId);
    Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
    Task<bool> UpdateProductAsync(ProductUpdateRequest product);
    Task<bool> DeleteProductAsync(int productId);
}
