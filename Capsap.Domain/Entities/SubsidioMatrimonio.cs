using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    // ==========================================
    // SUBSIDIO POR MATRIMONIO
    // ==========================================
    public class SubsidioMatrimonio : EntityBase
    {
        public int SolicitudSubsidioId { get; set; }
        public DateTime FechaCelebracion { get; set; }
        public string ActaNumero { get; set; }
        public string Tomo { get; set; }
        public string Anio { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }
        public string DNIConyuge { get; set; }
        public string CUILConyuge { get; set; }
        public bool AmbosAfiliadosActivos { get; set; }

        // Validación de plazo (180 días corridos desde la celebración)
        public bool DentroDePlazo => (DateTime.Now - FechaCelebracion).TotalDays <= 180;

        public virtual SolicitudSubsidio Solicitud { get; set; }
    }

}
