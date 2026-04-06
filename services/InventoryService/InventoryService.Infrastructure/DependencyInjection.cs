using FluentValidation;
using InventoryService.Application.Common.Behaviors;
using InventoryService.Application.Common.Interfaces;
using InventoryService.Infrastructure.Persistence;
using InventoryService.Infrastructure.Persistence.Repositories;
using InventoryService.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(opts =>
            opts.UseNpgsql(
                configuration.GetConnectionString("InventoryDb")));

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<InventoryDbContext>());

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBatchRepository,   BatchRepository>();
        services.AddScoped<IQrService,         QrService>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(InventoryService.Application.Features.Products
                       .Commands.AddProductManual.AddProductManualCommand).Assembly));

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(
            typeof(InventoryService.Application.Features.Products
                   .Commands.AddProductManual.AddProductManualCommandValidator).Assembly);

        return services;
    }
}