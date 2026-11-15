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
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductAddRequest product)
    {
        var addedProduct = await productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProductById), new { id = addedProduct.Id }, addedProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateRequest product)
    {
        if (id != product.Id)
        {
            return BadRequest("Product ID mismatch.");
        }

        var isUpdated = await productService.UpdateProductAsync(product);
        if (!isUpdated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var isDeleted = await productService.DeleteProductAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
