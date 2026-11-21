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

    ///api/inventory/adjust
    [HttpPut("adjust")]
    public async Task<IActionResult> AdjustInventory(AdjustAddRequest request, CancellationToken cancellationToken)
    {
        var isAdjusted = await _productService.AdjustProduct(request, cancellationToken);
        if (isAdjusted)
        {
            return Ok(new
            {
                Message = $"Product No. {request.ProductId} adjusted with quantity {request.NewQuantity}"
            });
        }
        return StatusCode(409, new
        {
            Message = $"Failed to adjust Product No. {request.ProductId}"
        });
    }

}