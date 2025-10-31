using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    // ==========================================
    // SERVICIO DE NOTIFICACIONES DE NEGOCIO
    // ==========================================
    public interface INotificacionesDomainService
    {
        List<string> ObtenerDestinatariosNotificacion(SolicitudSubsidio solicitud, TipoNotificacionNegocio tipo);
        string GenerarAsuntoEmail(TipoNotificacionNegocio tipo, SolicitudSubsidio solicitud);
        Dictionary<string, object> GenerarDatosNotificacion(SolicitudSubsidio solicitud);
    }
}
