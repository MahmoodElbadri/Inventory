using System;
using AutoMapper;
using Inventory.Application.Products.DTOs;
using InventoryApi.Models;

namespace Inventory.Application.Products.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductAddRequest, Product>();
        CreateMap<ProductUpdateRequest, Product>();
        CreateMap<Product, ProductResponse>();
    }
}
