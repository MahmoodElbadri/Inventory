using System;
using Inventory.Application.Common;
using Inventory.Application.Products.Interfaces;
using Inventory.Infrastructure.Data;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class ProductRepository(InventoryDbContext db) : IProductRepository
{
    public async Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        await db.Products.AddAsync(product, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<bool> DeleteProductAsync(int productId, CancellationToken cancellationToken)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);
        if (product == null) return false;
        product.isDeleted = true;
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<PagedResult<Product>> GetAllProductsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        if(pageNumber < 1 || pageSize < 1)
        {
            pageNumber = 1;
            pageSize = 10;
        }
        var query = db.Products.Where(p => p.isDeleted == false);
        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Product>(products, totalCount, pageNumber, pageSize);
    }

    public async Task<Product?> GetProductByIdAsync(int productId, CancellationToken cancellationToken)
    {
        return await db.Products.FirstOrDefaultAsync(p => p.Id == productId && p.isDeleted == false, cancellationToken);
    }

    public async Task<bool> UpdateProductAsync(int id, Product product, CancellationToken cancellationToken)
    {
        var existingProduct = await db.Products.FirstOrDefaultAsync(p => p.Id == id && p.isDeleted == false, cancellationToken);
        if (existingProduct == null)
        {
            return false;
        }

        existingProduct.ProductName = product.ProductName;
        existingProduct.Price = product.Price > 0 ? product.Price : existingProduct.Price;
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}

