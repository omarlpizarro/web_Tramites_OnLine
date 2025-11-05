using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // ==========================================
    // SERVICIO DE NOTIFICACIONES
    // ==========================================
    public interface INotificacionService
    {
        Task EnviarNotificacionSolicitudCreadaAsync(int solicitudId);
        Task EnviarNotificacionSolicitudEnviadaAsync(int solicitudId);
        Task EnviarNotificacionSolicitudAprobadaAsync(int solicitudId);
        Task EnviarNotificacionSolicitudRechazadaAsync(int solicitudId, string motivo);
        Task EnviarNotificacionDocumentacionIncompletaAsync(int solicitudId, List<string> documentosFaltantes);
        Task EnviarNotificacionNuevaSolicitudAEmpleadosAsync(int solicitudId);
    }
}
