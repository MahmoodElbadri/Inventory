using System;
using Inventory.Application.Products.Interfaces;
using Inventory.Infrastructure.Data;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class ProductRepository(InventoryDbContext db) : IProductRepository
{
    public async Task<Product> AddProductAsync(Product product)
    {
        await db.Products.AddAsync(product);
        await db.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null) return false;

        db.Products.Remove(product);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await db.Products.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        return await db.Products.FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        db.Products.Update(product);
        await db.SaveChangesAsync();
        return true;
    }
}

