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
    // SUBSIDIO MATRIMONIO CONFIGURATION
    // ==========================================
    public class SubsidioMatrimonioConfiguration : IEntityTypeConfiguration<SubsidioMatrimonio>
    {
        public void Configure(EntityTypeBuilder<SubsidioMatrimonio> builder)
        {
            builder.ToTable("SubsidiosMatrimonio");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.ActaNumero).HasMaxLength(50);
            builder.Property(s => s.Tomo).HasMaxLength(50);
            builder.Property(s => s.Anio).HasMaxLength(10);
            builder.Property(s => s.Ciudad).HasMaxLength(100);
            builder.Property(s => s.Provincia).HasMaxLength(100);
            builder.Property(s => s.DNIConyuge).HasMaxLength(20);
            builder.Property(s => s.CUILConyuge).HasMaxLength(20);

            // Relación 1:1 con SolicitudSubsidio
            builder.HasOne(s => s.Solicitud)
                .WithOne(sol => sol.SubsidioMatrimonio)
                .HasForeignKey<SubsidioMatrimonio>(s => s.SolicitudSubsidioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => s.SolicitudSubsidioId).IsUnique();
        }
    }
}
