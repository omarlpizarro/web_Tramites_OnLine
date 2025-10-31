using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    // ==========================================
    // SOLICITUD DE SUBSIDIO (Entidad Principal)
    // ==========================================
    public class SolicitudSubsidio : EntityBase
    {
        public string NumeroSolicitud { get; set; } // Generado automáticamente
        public int AfiliadoSolicitanteId { get; set; }
        public int? AfiliadoConyuge2Id { get; set; } // Si ambos son afiliados
        public TipoSubsidio TipoSubsidio { get; set; }
        public EstadoSolicitud Estado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesInternas { get; set; } // Para uso administrativo

        // Datos Bancarios
        public string CBU { get; set; }
        public string TipoCuenta { get; set; }
        public string Banco { get; set; }
        public string TitularCuenta { get; set; }
        public string CUITTitular { get; set; }
        public bool TransferenciaATercero { get; set; }

        // Relaciones
        public virtual Afiliado AfiliadoSolicitante { get; set; }
        public virtual Afiliado AfiliadoConyuge2 { get; set; }
        public virtual ICollection<DocumentoAdjunto> Documentos { get; set; }
        public virtual ICollection<HistorialSolicitud> Historial { get; set; }

        // Navegación específica por tipo de subsidio
        public virtual SubsidioMatrimonio SubsidioMatrimonio { get; set; }
        public virtual SubsidioMaternidad SubsidioMaternidad { get; set; }
        public virtual SubsidioNacimientoAdopcion SubsidioNacimientoAdopcion { get; set; }
        public virtual SubsidioHijoDiscapacitado SubsidioHijoDiscapacitado { get; set; }
    }

}
