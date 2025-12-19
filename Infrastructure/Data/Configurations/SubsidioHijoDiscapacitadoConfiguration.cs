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
    // SUBSIDIO HIJO DISCAPACITADO CONFIGURATION
    // ==========================================
    public class SubsidioHijoDiscapacitadoConfiguration : IEntityTypeConfiguration<SubsidioHijoDiscapacitado>
    {
        public void Configure(EntityTypeBuilder<SubsidioHijoDiscapacitado> builder)
        {
            builder.ToTable("SubsidiosHijoDiscapacitado");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Nombre).IsRequired().HasMaxLength(200);
            builder.Property(s => s.DNI).HasMaxLength(20);
            builder.Property(s => s.CUIL).HasMaxLength(20);
            builder.Property(s => s.NumeroCertificadoDiscapacidad).HasMaxLength(100);
            builder.Property(s => s.LugarEmision).HasMaxLength(200);

            // Relación 1:1 con SolicitudSubsidio
            builder.HasOne(s => s.Solicitud)
                .WithOne(sol => sol.SubsidioHijoDiscapacitado)
                .HasForeignKey<SubsidioHijoDiscapacitado>(s => s.SolicitudSubsidioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => s.SolicitudSubsidioId).IsUnique();
        }
    }
}
