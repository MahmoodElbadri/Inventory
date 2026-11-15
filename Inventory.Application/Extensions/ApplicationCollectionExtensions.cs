using Microsoft.Extensions.DependencyInjection;
using Inventory.Application.Products.Interfaces;
using AutoMapper;
using Inventory.Application.Products.Services;


namespace Inventory.Application.Extensions;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(
    cfg => { },
    typeof(ApplicationCollectionExtensions).Assembly
);
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}
