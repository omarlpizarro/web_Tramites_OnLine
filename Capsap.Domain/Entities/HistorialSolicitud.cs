using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    // ==========================================
    // HISTORIAL DE SOLICITUD (Auditoría)
    // ==========================================
    public class HistorialSolicitud : EntityBase
    {
        public int SolicitudSubsidioId { get; set; }
        public EstadoSolicitud EstadoAnterior { get; set; }
        public EstadoSolicitud EstadoNuevo { get; set; }
        public DateTime FechaCambio { get; set; }
        public int UsuarioId { get; set; }
        public string Comentario { get; set; }
        public string DatosAdicionales { get; set; } // JSON con información extra

        public virtual SolicitudSubsidio Solicitud { get; set; }
        public virtual Usuario Usuario { get; set; }
    }

}
