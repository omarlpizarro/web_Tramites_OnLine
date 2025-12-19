using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    // ==========================================
    // USUARIO DEL SISTEMA
    // ==========================================
    public class Usuario : EntityBase
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public RolUsuario Rol { get; set; }
        public int? AfiliadoId { get; set; } // Si es un afiliado
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public bool EmailConfirmado { get; set; }
        public DateTime? UltimoAcceso { get; set; }

        public virtual Afiliado Afiliado { get; set; }
        public virtual ICollection<HistorialSolicitud> AccionesRealizadas { get; set; }
    }

}
