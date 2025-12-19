using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capsap.Domain.Entities;
using Capsap.Domain.Enums;

namespace Capsap.Domain.Interfaces.Services
{
    // ==========================================
    // SERVICIO DE NOTIFICACIONES
    // ==========================================
    public interface INotificacionService
    {
        Task EnviarNotificacionAsync(int usuarioId, string titulo, string mensaje, TipoNotificacion tipo);
        Task<IEnumerable<Notificacion>> ObtenerNotificacionesUsuarioAsync(int usuarioId, bool soloNoLeidas = false);
        Task MarcarComoLeidaAsync(int notificacionId);
        Task MarcarTodasComoLeidasAsync(int usuarioId);
        Task<int> ContarNotificacionesNoLeidasAsync(int usuarioId);
    }
}
