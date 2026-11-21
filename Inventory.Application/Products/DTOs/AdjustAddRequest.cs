using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Products.DTOs;

/*
 * "productId": 1,
  "newQuantity": 25,
  "reason": "Physical stock count"
 */
public class AdjustAddRequest
{
 public int ProductId { get; set; }
 public int NewQuantity { get; set; }
 public string Reason { get; set; } 
}
