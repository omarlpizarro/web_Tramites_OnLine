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
    // HIJO NACIMIENTO/ADOPCIÓN CONFIGURATION
    // ==========================================
    public class HijoNacimientoAdopcionConfiguration : IEntityTypeConfiguration<HijoNacimientoAdopcion>
    {
        public void Configure(EntityTypeBuilder<HijoNacimientoAdopcion> builder)
        {
            builder.ToTable("HijosNacimientoAdopcion");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Nombre).IsRequired().HasMaxLength(200);
            builder.Property(h => h.DNI).HasMaxLength(20);
            builder.Property(h => h.CUIL).HasMaxLength(20);
            builder.Property(h => h.ActaNumero).HasMaxLength(50);
            builder.Property(h => h.Tomo).HasMaxLength(50);
            builder.Property(h => h.Anio).HasMaxLength(10);
            builder.Property(h => h.CiudadNacimiento).HasMaxLength(100);
            builder.Property(h => h.ProvinciaNacimiento).HasMaxLength(100);
        }
    }

}
