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
    // CONFIGURACIÓN SISTEMA CONFIGURATION
    // ==========================================
    public class ConfiguracionSistemaConfiguration : IEntityTypeConfiguration<ConfiguracionSistema>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionSistema> builder)
        {
            builder.ToTable("ConfiguracionSistema");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Clave).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Descripcion).HasMaxLength(500);
            builder.Property(c => c.TipoDato).HasMaxLength(50);

            builder.HasIndex(c => c.Clave).IsUnique();
        }
    }
}
