using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    // ==========================================
    // IEmailService
    // ==========================================
    public interface IEmailService
    {
        Task<bool> EnviarEmailAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true);
        Task<bool> EnviarConfirmacionSolicitudAsync(string emailAfiliado, string numeroSolicitud, string nombreAfiliado);
        Task<bool> EnviarNotificacionCambioEstadoAsync(string emailAfiliado, string numeroSolicitud, EstadoSolicitud nuevoEstado, string? comentario = null);
        Task<bool> EnviarSolicitudDocumentacionAsync(string emailAfiliado, string numeroSolicitud, List<string> documentosFaltantes);
        Task<bool> EnviarRecordatorioDocumentacionAsync(string emailAfiliado, string numeroSolicitud);
        Task<bool> EnviarNotificacionAprobacionAsync(string emailAfiliado, string numeroSolicitud, decimal monto);
        Task<bool> EnviarNotificacionRechazoAsync(string emailAfiliado, string numeroSolicitud, string motivo);
        Task<bool> EnviarEmailConAdjuntoAsync(string destinatario, string asunto, string cuerpo, string rutaAdjunto, bool esHtml = true);
    }
}
