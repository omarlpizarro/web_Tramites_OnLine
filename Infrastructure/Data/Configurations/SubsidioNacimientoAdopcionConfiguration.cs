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
    // SUBSIDIO NACIMIENTO/ADOPCIÓN CONFIGURATION
    // ==========================================
    public class SubsidioNacimientoAdopcionConfiguration : IEntityTypeConfiguration<SubsidioNacimientoAdopcion>
    {
        public void Configure(EntityTypeBuilder<SubsidioNacimientoAdopcion> builder)
        {
            builder.ToTable("SubsidiosNacimientoAdopcion");

            builder.HasKey(s => s.Id);

            // Relación 1:1 con SolicitudSubsidio
            builder.HasOne(s => s.Solicitud)
                .WithOne(sol => sol.SubsidioNacimientoAdopcion)
                .HasForeignKey<SubsidioNacimientoAdopcion>(s => s.SolicitudSubsidioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación 1:N con Hijos
            builder.HasMany(s => s.Hijos)
                .WithOne(h => h.SubsidioNacimiento)
                .HasForeignKey(h => h.SubsidioNacimientoAdopcionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => s.SolicitudSubsidioId).IsUnique();
        }
    }
}
