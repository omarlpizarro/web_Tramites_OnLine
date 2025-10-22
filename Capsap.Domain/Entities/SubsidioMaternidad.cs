using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    // ==========================================
    // SUBSIDIO POR MATERNIDAD
    // ==========================================
    public class SubsidioMaternidad : EntityBase
    {
        public int SolicitudSubsidioId { get; set; }
        public TipoEventoMaternidad TipoEvento { get; set; } // Nacimiento o Adopción
        public DateTime FechaEvento { get; set; } // Fecha nacimiento o sentencia
        public string ActaNumero { get; set; }
        public string Tomo { get; set; }
        public string Anio { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }

        // Datos del hijo/a
        public string NombreHijo { get; set; }
        public string DNIHijo { get; set; }
        public string CUILHijo { get; set; }

        // Adopción
        public DateTime? FechaSentenciaJudicial { get; set; }
        public string CiudadSentencia { get; set; }
        public string ProvinciaSentencia { get; set; }

        public bool DentroDePlazo => (DateTime.Now - FechaEvento).TotalDays <= 180;

        public virtual SolicitudSubsidio Solicitud { get; set; }
    }

}
