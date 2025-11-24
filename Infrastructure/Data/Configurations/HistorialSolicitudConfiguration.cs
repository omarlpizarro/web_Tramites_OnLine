using Capsap.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    // ==========================================
    // HISTORIAL SOLICITUD CONFIGURATION
    // ==========================================
    public class HistorialSolicitudConfiguration : IEntityTypeConfiguration<HistorialSolicitud>
    {
        public void Configure(EntityTypeBuilder<HistorialSolicitud> builder)
        {
            builder.ToTable("HistorialSolicitudes");

            builder.HasKey(h => h.Id);

            // Relaciones
            builder.HasOne(h => h.Solicitud)
                .WithMany(s => s.Historial)
                .HasForeignKey(h => h.SolicitudSubsidioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(h => h.Usuario)
                .WithMany(u => u.AccionesRealizadas)
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(h => h.SolicitudSubsidioId);
            builder.HasIndex(h => h.FechaCambio);
        }
    }
}
