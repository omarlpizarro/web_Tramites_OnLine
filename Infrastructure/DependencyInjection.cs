using Capsap.Domain.Interfaces.Repositories;
using Capsap.Domain.Interfaces.Services;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Identity
            services.AddIdentityConfiguration();

            // Repositorios
            services.AddScoped<IAfiliadoRepository, AfiliadoRepository>();
            services.AddScoped<ISolicitudSubsidioRepository, SolicitudSubsidioRepository>();
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();

            // Servicios
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IPdfGeneratorService, PdfGeneratorService>();

            return services;
        }

        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Aplicar migraciones pendientes
            await context.Database.MigrateAsync();

            // Seed de roles y usuario admin
            await IdentityConfiguration.SeedRolesAndAdminAsync(serviceProvider);
        }
    }
}
