using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Events
{
    // ==========================================
    // EVENTOS ESPECÍFICOS
    // ==========================================

    public class SolicitudCreadaEvent : DomainEvent
    {
        public int SolicitudId { get; set; }
        public int AfiliadoId { get; set; }
        public TipoSubsidio TipoSubsidio { get; set; }
        public string NumeroSolicitud { get; set; }

        public SolicitudCreadaEvent(int solicitudId, int afiliadoId, TipoSubsidio tipoSubsidio, string numeroSolicitud)
        {
            SolicitudId = solicitudId;
            AfiliadoId = afiliadoId;
            TipoSubsidio = tipoSubsidio;
            NumeroSolicitud = numeroSolicitud;
        }
    }

}
