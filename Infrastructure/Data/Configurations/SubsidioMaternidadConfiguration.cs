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
    // SUBSIDIO MATERNIDAD CONFIGURATION
    // ==========================================
    public class SubsidioMaternidadConfiguration : IEntityTypeConfiguration<SubsidioMaternidad>
    {
        public void Configure(EntityTypeBuilder<SubsidioMaternidad> builder)
        {
            builder.ToTable("SubsidiosMaternidad");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.NombreHijo).HasMaxLength(200);
            builder.Property(s => s.DNIHijo).HasMaxLength(20);
            builder.Property(s => s.CUILHijo).HasMaxLength(20);
            builder.Property(s => s.ActaNumero).HasMaxLength(50);
            builder.Property(s => s.Tomo).HasMaxLength(50);
            builder.Property(s => s.Anio).HasMaxLength(10);
            builder.Property(s => s.Ciudad).HasMaxLength(100);
            builder.Property(s => s.Provincia).HasMaxLength(100);

            // Relación 1:1 con SolicitudSubsidio
            builder.HasOne(s => s.Solicitud)
                .WithOne(sol => sol.SubsidioMaternidad)
                .HasForeignKey<SubsidioMaternidad>(s => s.SolicitudSubsidioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => s.SolicitudSubsidioId).IsUnique();
        }
    }
}
