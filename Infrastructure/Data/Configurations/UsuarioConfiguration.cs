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
    // USUARIO CONFIGURATION
    // ==========================================
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            // Relación con Afiliado (opcional)
            builder.HasOne(u => u.Afiliado)
                .WithOne(a => a.Usuario)
                .HasForeignKey<Usuario>(u => u.AfiliadoId)
                .IsRequired(false);

            // Índices
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Rol);
        }
    }
}
