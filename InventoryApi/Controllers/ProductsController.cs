using Inventory.Application.Products.DTOs;
using Inventory.Application.Products.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducts(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var products = await productService.GetAllProductsAsync(pageNumber, pageSize, cancellationToken);
        
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
    {
        var product = await productService.GetProductByIdAsync(id, cancellationToken);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductAddRequest product, CancellationToken cancellationToken)
    {
        var addedProduct = await productService.AddProductAsync(product, cancellationToken);
        return CreatedAtAction(nameof(GetProductById), new { id = addedProduct.Id }, addedProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateRequest product, CancellationToken cancellationToken)
    {
        if (id != product.Id)
        {
            return BadRequest("Product ID mismatch.");
        }

        var isUpdated = await productService.UpdateProductAsync(product, cancellationToken);
        if (!isUpdated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
    {
        var isDeleted = await productService.DeleteProductAsync(id, cancellationToken);
        if (!isDeleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
