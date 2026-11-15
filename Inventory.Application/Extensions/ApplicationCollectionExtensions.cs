using AutoMapper;
using FluentValidation;
using Inventory.Application.Products.Interfaces;
using Inventory.Application.Products.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Inventory.Application.Extensions;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(
            cfg => { },
            typeof(ApplicationCollectionExtensions).Assembly);
        services.AddScoped<IProductService, ProductService>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }

}
