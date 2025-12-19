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
    // AFILIADO CONFIGURATION
    // ==========================================
    public class AfiliadoConfiguration : IEntityTypeConfiguration<Afiliado>
    {
        public void Configure(EntityTypeBuilder<Afiliado> builder)
        {
            builder.ToTable("Afiliados");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.MatriculaProfesional)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.DNI)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.CUIL)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.Telefono)
                .HasMaxLength(50);

            builder.Property(a => a.Domicilio)
                .HasMaxLength(255);

            builder.Property(a => a.Ciudad)
                .HasMaxLength(100);

            builder.Property(a => a.Provincia)
                .HasMaxLength(100);

            // Índices
            builder.HasIndex(a => a.MatriculaProfesional).IsUnique();
            builder.HasIndex(a => a.DNI).IsUnique();
            builder.HasIndex(a => a.Email);
        }
    }
}
