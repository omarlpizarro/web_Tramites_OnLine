using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Capsap.Domain.Entities;
using Infrastructure.Data.Configurations;

namespace Infrastructure.Data
{
    public class CapsapDbContext : DbContext
    {
        public CapsapDbContext(DbContextOptions<CapsapDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Afiliado> Afiliados { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<SolicitudSubsidio> SolicitudesSubsidio { get; set; }
        public DbSet<SubsidioMatrimonio> SubsidiosMatrimonio { get; set; }
        public DbSet<SubsidioMaternidad> SubsidiosMaternidad { get; set; }
        public DbSet<SubsidioNacimientoAdopcion> SubsidiosNacimientoAdopcion { get; set; }
        public DbSet<HijoNacimientoAdopcion> HijosNacimientoAdopcion { get; set; }
        public DbSet<SubsidioHijoDiscapacitado> SubsidiosHijoDiscapacitado { get; set; }
        public DbSet<DocumentoAdjunto> DocumentosAdjuntos { get; set; }
        public DbSet<HistorialSolicitud> HistorialSolicitudes { get; set; }
        public DbSet<ConfiguracionSistema> ConfiguracionSistema { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar configuraciones
            modelBuilder.ApplyConfiguration(new AfiliadoConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new SolicitudSubsidioConfiguration());
            modelBuilder.ApplyConfiguration(new SubsidioMatrimonioConfiguration());
            modelBuilder.ApplyConfiguration(new SubsidioMaternidadConfiguration());
            modelBuilder.ApplyConfiguration(new SubsidioNacimientoAdopcionConfiguration());
            modelBuilder.ApplyConfiguration(new HijoNacimientoAdopcionConfiguration());
            modelBuilder.ApplyConfiguration(new SubsidioHijoDiscapacitadoConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentoAdjuntoConfiguration());
            modelBuilder.ApplyConfiguration(new HistorialSolicitudConfiguration());
            modelBuilder.ApplyConfiguration(new ConfiguracionSistemaConfiguration());

            // Seed inicial de datos
            SeedData(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Actualizar automáticamente FechaModificacion
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is EntityBase &&
                           (e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                ((EntityBase)entry.Entity).FechaModificacion = DateTime.Now;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Configuración inicial del sistema
            modelBuilder.Entity<ConfiguracionSistema>().HasData(
                new ConfiguracionSistema { Id = 1, Clave = "Subsidio.Matrimonio.PlazoMaximoDias", Valor = "180", Descripcion = "Plazo máximo en días para solicitar subsidio por matrimonio", TipoDato = "int", FechaCreacion = DateTime.Now, Activo = true },
                new ConfiguracionSistema { Id = 2, Clave = "Subsidio.Maternidad.PlazoMaximoDias", Valor = "180", Descripcion = "Plazo máximo en días para solicitar subsidio por maternidad", TipoDato = "int", FechaCreacion = DateTime.Now, Activo = true },
                new ConfiguracionSistema { Id = 3, Clave = "Subsidio.Nacimiento.PlazoMaximoDias", Valor = "180", Descripcion = "Plazo máximo en días para solicitar subsidio por nacimiento/adopción", TipoDato = "int", FechaCreacion = DateTime.Now, Activo = true },
                new ConfiguracionSistema { Id = 4, Clave = "Documentos.TamanoMaximoMB", Valor = "10", Descripcion = "Tamaño máximo de archivo en MB", TipoDato = "decimal", FechaCreacion = DateTime.Now, Activo = true },
                new ConfiguracionSistema { Id = 5, Clave = "Documentos.FormatosPermitidos", Valor = "pdf,jpg,jpeg,png", Descripcion = "Formatos de archivo permitidos", TipoDato = "string", FechaCreacion = DateTime.Now, Activo = true },
                new ConfiguracionSistema { Id = 6, Clave = "Sistema.EmailNotificaciones", Valor = "notificaciones@capsap.gov.ar", Descripcion = "Email para notificaciones del sistema", TipoDato = "string", FechaCreacion = DateTime.Now, Activo = true }
            );
        }
    }
}
