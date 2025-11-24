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
    // DOCUMENTO ADJUNTO CONFIGURATION
    // ==========================================
    public class DocumentoAdjuntoConfiguration : IEntityTypeConfiguration<DocumentoAdjunto>
    {
        public void Configure(EntityTypeBuilder<DocumentoAdjunto> builder)
        {
            builder.ToTable("DocumentosAdjuntos");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.NombreArchivo).IsRequired().HasMaxLength(255);
            builder.Property(d => d.RutaArchivo).IsRequired().HasMaxLength(500);
            builder.Property(d => d.ContentType).HasMaxLength(100);

            // Relaciones
            builder.HasOne(d => d.Solicitud)
                .WithMany(s => s.Documentos)
                .HasForeignKey(d => d.SolicitudSubsidioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.CertificadoPor)
                .WithMany()
                .HasForeignKey(d => d.CertificadoPorUsuarioId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(d => d.SolicitudSubsidioId);
            builder.HasIndex(d => d.TipoDocumento);
        }
    }
}
