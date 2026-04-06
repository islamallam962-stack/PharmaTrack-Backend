using FluentValidation;
using MarketplaceService.Application.Common.Behaviors;
using MarketplaceService.Application.Common.Interfaces;
using MarketplaceService.Infrastructure.Persistence;
using MarketplaceService.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketplaceService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MarketplaceDbContext>(opts =>
            opts.UseNpgsql(
                configuration.GetConnectionString("MarketplaceDb")));

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<MarketplaceDbContext>());

        services.AddScoped<IListingRepository, ListingRepository>();
        services.AddScoped<IRequestRepository, RequestRepository>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(MarketplaceService.Application.Features.Marketplace
                       .Commands.CreateListing.CreateListingCommand).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>),
                              typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(
            typeof(MarketplaceService.Application.Features.Marketplace
                   .Commands.CreateListing.CreateListingCommandValidator).Assembly);

        return services;
    }
}