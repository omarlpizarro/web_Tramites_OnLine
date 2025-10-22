using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{

    // ==========================================
    // AFILIADO (Matriculado/Abogado)
    // ==========================================
    public class Afiliado : EntityBase
    {
        public string MatriculaProfesional { get; set; } // M.P.
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string DNI { get; set; }
        public string CUIL { get; set; }
        public EstadoCivil EstadoCivil { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Domicilio { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }
        public bool TieneDeuda { get; set; } // Art. 73 Ley 4764/94
        public DateTime? FechaUltimaVerificacionDeuda { get; set; }

        // Relaciones
        public virtual ICollection<SolicitudSubsidio> Solicitudes { get; set; }
        public virtual Usuario Usuario { get; set; }
    }

}
