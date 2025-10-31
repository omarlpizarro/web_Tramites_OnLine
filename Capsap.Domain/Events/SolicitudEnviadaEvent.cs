using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Events
{
    public class SolicitudEnviadaEvent : DomainEvent
    {
        public int SolicitudId { get; set; }
        public string NumeroSolicitud { get; set; }
        public int AfiliadoId { get; set; }

        public SolicitudEnviadaEvent(int solicitudId, string numeroSolicitud, int afiliadoId)
        {
            SolicitudId = solicitudId;
            NumeroSolicitud = numeroSolicitud;
            AfiliadoId = afiliadoId;
        }
    }

}
