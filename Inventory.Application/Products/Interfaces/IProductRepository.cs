using System;
using InventoryApi.Models;

namespace Inventory.Application.Products.Interfaces;

public interface IProductRepository
{
    Task<Product> AddProductAsync(Product product);
    Task<Product?> GetProductByIdAsync(int productId);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int productId);
}
