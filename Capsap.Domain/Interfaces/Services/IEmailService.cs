using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Services
{
    // ==========================================
    // SERVICIO DE EMAIL
    // =========================================
    public interface IEmailService
    {
        Task EnviarEmailAsync(string destinatario, string asunto, string cuerpoHtml);
        Task EnviarEmailConAdjuntoAsync(string destinatario, string asunto, string cuerpoHtml, byte[] adjunto, string nombreArchivo);

        // Emails específicos del negocio
        Task EnviarEmailSolicitudCreadaAsync(SolicitudSubsidio solicitud);
        Task EnviarEmailSolicitudEnviadaAsync(SolicitudSubsidio solicitud);
        Task EnviarEmailSolicitudAprobadaAsync(SolicitudSubsidio solicitud);
        Task EnviarEmailSolicitudRechazadaAsync(SolicitudSubsidio solicitud, string motivo);
        Task EnviarEmailDocumentacionIncompletaAsync(SolicitudSubsidio solicitud, List<string> documentosFaltantes);
        Task EnviarEmailNuevaSolicitudAEmpleadosAsync(SolicitudSubsidio solicitud);
        Task EnviarEmailRecuperacionPasswordAsync(string email, string token);
        Task EnviarEmailConfirmacionCuentaAsync(string email, string token);
    }
}
