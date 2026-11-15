using System;
using Inventory.Application.Common;
using InventoryApi.Models;

namespace Inventory.Application.Products.Interfaces;

public interface IProductRepository
{
    Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> GetProductByIdAsync(int productId, CancellationToken cancellationToken);
    Task<PagedResult<Product>> GetAllProductsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<bool> UpdateProductAsync(int id ,Product product, CancellationToken cancellationToken);
    Task<bool> DeleteProductAsync(int productId, CancellationToken cancellationToken);
}
