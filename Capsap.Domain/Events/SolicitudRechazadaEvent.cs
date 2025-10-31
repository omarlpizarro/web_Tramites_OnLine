using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Events
{
    public class SolicitudRechazadaEvent : DomainEvent
    {
        public int SolicitudId { get; set; }
        public string NumeroSolicitud { get; set; }
        public int AfiliadoId { get; set; }
        public int RechazadaPorUsuarioId { get; set; }
        public string Motivo { get; set; }

        public SolicitudRechazadaEvent(int solicitudId, string numeroSolicitud, int afiliadoId, int rechazadaPorUsuarioId, string motivo)
        {
            SolicitudId = solicitudId;
            NumeroSolicitud = numeroSolicitud;
            AfiliadoId = afiliadoId;
            RechazadaPorUsuarioId = rechazadaPorUsuarioId;
            Motivo = motivo;
        }
    }

}
