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
    // SOLICITUD SUBSIDIO CONFIGURATION
    // ==========================================
    public class SolicitudSubsidioConfiguration : IEntityTypeConfiguration<SolicitudSubsidio>
    {
        public void Configure(EntityTypeBuilder<SolicitudSubsidio> builder)
        {
            builder.ToTable("SolicitudesSubsidio");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.NumeroSolicitud)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.CBU)
                .IsRequired()
                .HasMaxLength(22);

            builder.Property(s => s.TipoCuenta)
                .HasMaxLength(50);

            builder.Property(s => s.Banco)
                .HasMaxLength(100);

            builder.Property(s => s.TitularCuenta)
                .HasMaxLength(200);

            builder.Property(s => s.CUITTitular)
                .HasMaxLength(20);

            // Relaciones
            builder.HasOne(s => s.AfiliadoSolicitante)
                .WithMany(a => a.Solicitudes)
                .HasForeignKey(s => s.AfiliadoSolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.AfiliadoConyuge2)
                .WithMany()
                .HasForeignKey(s => s.AfiliadoConyuge2Id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(s => s.NumeroSolicitud).IsUnique();
            builder.HasIndex(s => s.Estado);
            builder.HasIndex(s => s.AfiliadoSolicitanteId);
            builder.HasIndex(s => s.FechaSolicitud);
            builder.HasIndex(s => new { s.TipoSubsidio, s.Estado });
        }
    }
}
