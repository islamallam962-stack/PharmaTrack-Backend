using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PharmacyService.Application.Common.Behaviors;
using PharmacyService.Application.Common.Interfaces;
using PharmacyService.Infrastructure.Persistence;
using PharmacyService.Infrastructure.Persistence.Repositories;

namespace PharmacyService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PharmacyDbContext>(opts =>
            opts.UseNpgsql(
                configuration.GetConnectionString("PharmacyDb")));

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<PharmacyDbContext>());

        services.AddScoped<IPharmacyRepository, PharmacyRepository>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(PharmacyService.Application.Features.Pharmacies
                       .Commands.CreatePharmacy.CreatePharmacyCommand).Assembly));

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(
            typeof(PharmacyService.Application.Features.Pharmacies
                   .Commands.CreatePharmacy.CreatePharmacyCommandValidator).Assembly);

        return services;
    }
}