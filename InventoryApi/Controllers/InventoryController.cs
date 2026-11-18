using System.Reflection.Metadata.Ecma335;
using Inventory.Application.Products.DTOs;
using Inventory.Application.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController(IProductService _productService) : ControllerBase
{
    //	/api/inventory/restock
    [HttpPost("restock")]
    public async Task<IActionResult> RestockInventory(RestockAddRequest request, CancellationToken cancellationToken)
    {
        var isStocked = await _productService.RestockProduct(request, cancellationToken);
        if (isStocked)
        {
            return Ok(new
            {
                Message = $"Product No. {request.ProductId} restocked with quantity {request.Quantity}"
            });
        }
        return StatusCode(409, new
        {
            Message = $"Failed to restock Product No. {request.ProductId}"
        });
    }

}