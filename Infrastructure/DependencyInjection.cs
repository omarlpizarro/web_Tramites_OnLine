using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;                // Para CapsapDbContext
using Infrastructure.Identity;            // Para IdentityConfiguration
using Infrastructure.Repositories;        // Para los repositorios
using Infrastructure.Services;
using Application.Interfaces.Services;
using Application.Interfaces;            // Para los servicios

namespace Capsap.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<CapsapDbContext>(options =>
        options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(CapsapDbContext).Assembly.FullName)));

        // Identity
        services.AddIdentityConfiguration();

        // Repositorios
        services.AddScoped<IAfiliadoRepository, AfiliadoRepository>();
        //services.AddScoped<ISolicitudSubsidioRepository, SolicitudSubsidioRepository>();
        services.AddScoped<IDocumentoRepository, DocumentoRepository>();

        // Servicios
        services.AddScoped<IEmailService, EmailService>();
        //services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IPdfGeneratorService, PdfGeneratorService>();

        return services;
    }

    public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CapsapDbContext>();

        // Aplicar migraciones pendientes
        await context.Database.MigrateAsync();

        // Seed de roles y usuario admin
        await IdentityConfiguration.SeedRolesAndAdminAsync(serviceProvider);
    }
}