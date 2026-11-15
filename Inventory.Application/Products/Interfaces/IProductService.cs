using System;
using Inventory.Application.Common;
using Inventory.Application.Products.DTOs;

namespace Inventory.Application.Products.Interfaces;

public interface IProductService
{
    Task<ProductResponse> AddProductAsync(ProductAddRequest product, CancellationToken cancellationToken);
    Task<ProductResponse?> GetProductByIdAsync(int productId, CancellationToken cancellationToken);
    Task<PagedResult<ProductResponse>> GetAllProductsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<bool> UpdateProductAsync(ProductUpdateRequest product, CancellationToken cancellationToken);
    Task<bool> DeleteProductAsync(int productId, CancellationToken cancellationToken);
}
