using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? DNI { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaUltimaActividad { get; set; }
        public DateTime? FechaDesactivacion { get; set; }

        // Relación con Afiliado (si el usuario es un afiliado)
        public int? AfiliadoId { get; set; }

        public string NombreCompleto => $"{Apellido}, {Nombre}";
    }
}
