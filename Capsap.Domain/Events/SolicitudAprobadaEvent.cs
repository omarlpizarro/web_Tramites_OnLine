using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Events
{
    public class SolicitudAprobadaEvent : DomainEvent
    {
        public int SolicitudId { get; set; }
        public string NumeroSolicitud { get; set; }
        public int AfiliadoId { get; set; }
        public int AprobadaPorUsuarioId { get; set; }
        public string Comentario { get; set; }

        public SolicitudAprobadaEvent(int solicitudId, string numeroSolicitud, int afiliadoId, int aprobadaPorUsuarioId, string comentario)
        {
            SolicitudId = solicitudId;
            NumeroSolicitud = numeroSolicitud;
            AfiliadoId = afiliadoId;
            AprobadaPorUsuarioId = aprobadaPorUsuarioId;
            Comentario = comentario;
        }
    }

}
