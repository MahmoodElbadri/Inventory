using System;
using Inventory.Application.Common;
using Inventory.Application.Exceptions;
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
        var product =
            await db.Products.FirstOrDefaultAsync(p => p.Id == productId && p.isDeleted == false, cancellationToken);
        if (product == null) return false;
        product.isDeleted = true;
        // product.ProductName = product.ProductName + " (Deleted)";
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<PagedResult<Product>> GetAllProductsAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        if (pageNumber < 1 || pageSize < 1)
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

    public async Task<bool> IsProductNameExist(string name, CancellationToken cancellationToken)
    {
        return await db.Products.AnyAsync(p => p.ProductName == name && p.isDeleted == false, cancellationToken);
    }

    public async Task<bool> RestockProduct(int productId, int quantity, CancellationToken cancellationToken)
    {
        int maxRetryCount = 3;
        int maxAttempts = 3;
        int attempt = 0;

        for (int i = maxRetryCount; i > 0; i--)
        {
            try
            {
                var product =
                    await db.Products.FirstOrDefaultAsync(p => p.Id == productId && !p.isDeleted, cancellationToken);

                if (product is null)
                    throw new NotFoundException($"Product {productId} not found");

                await using var transactionProcess = await db.Database.BeginTransactionAsync(cancellationToken);

                // add a checkpoint to rollback to the latest checkpoint 
                await transactionProcess.CreateSavepointAsync("beforeRestock", cancellationToken);

                product.QuantityOnHand += quantity;

                db.InventoryTransactions.Add(new InventoryTransaction
                {
                    ProductId = productId,
                    QuantityChanged = quantity,
                    Type = TransactionType.Restock,
                    AdjustmentReason = $"Restocked product No. {productId} by {quantity} pieces at {DateTime.Now}"
                });
                //    await transactionProcess.RollbackToSavepointAsync("beforeRestock", cancellationToken);

                await db.SaveChangesAsync(cancellationToken);

                await transactionProcess.CommitAsync(cancellationToken);

                return true;
            }
            //    catch(DbUpdateConcurrencyException ex)
            //    {
            //         db.ChangeTracker.Clear();
            //        await Task.Delay(250, cancellationToken);
            //    }
            catch (DbUpdateConcurrencyException)
            {
                db.ChangeTracker.Clear();

                if (attempt == maxAttempts - 1)
                    return false;

                var delay = 100 * (1 << attempt)
                            + Random.Shared.Next(20, 100);

                await Task.Delay(delay, cancellationToken);
            }
        }

        return false;
    }

    public async Task<bool> AdjustProduct(int productId, int quantity, string reason,
        CancellationToken cancellationToken)
    {
        var product =
            await db.Products.FirstOrDefaultAsync(tmp => tmp.Id == productId && !tmp.isDeleted, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException($"Product {productId} not found");
        }
        if(product.QuantityOnHand == quantity)
        {
            return true;
        }

        int attempt = 0;
        int maxAttempts = 3;

        try
        {
            await using var transactionProcess = await db.Database.BeginTransactionAsync(cancellationToken);

            // add a checkpoint to rollback to the latest checkpoint 
            await transactionProcess.CreateSavepointAsync("beforeAdjust", cancellationToken);

            product.QuantityOnHand += quantity;


            db.InventoryTransactions.Add(new InventoryTransaction
            {
                ProductId = productId,
                QuantityChanged = quantity,
                Type = TransactionType.Adjustment,
                AdjustmentReason = reason
            });
            //    await transactionProcess.RollbackToSavepointAsync("beforeRestock", cancellationToken);

            await db.SaveChangesAsync(cancellationToken);

            await transactionProcess.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            db.ChangeTracker.Clear();

            if (attempt == maxAttempts - 1)
                return false;
            attempt++;

            var delay = 100 * (1 << attempt)
                        + Random.Shared.Next(20, 100);

            await Task.Delay(delay, cancellationToken);
        }


        return true;
    }

    public async Task<bool> UpdateProductAsync(int id, Product product, CancellationToken cancellationToken)
    {
        var existingProduct =
            await db.Products.FirstOrDefaultAsync(p => p.Id == id && p.isDeleted == false, cancellationToken);
        if (existingProduct == null)
        {
            return false;
        }

        existingProduct.ProductName = product.ProductName;
        existingProduct.Price = product.Price;
        existingProduct.Description = product.Description;

        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}

