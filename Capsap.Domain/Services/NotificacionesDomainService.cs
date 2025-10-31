using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using Capsap.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    public class NotificacionesDomainService : INotificacionesDomainService
    {
        public List<string> ObtenerDestinatariosNotificacion(SolicitudSubsidio solicitud, TipoNotificacionNegocio tipo)
        {
            var destinatarios = new List<string>();

            switch (tipo)
            {
                case TipoNotificacionNegocio.SolicitudCreada:
                case TipoNotificacionNegocio.SolicitudEnviada:
                case TipoNotificacionNegocio.SolicitudAprobada:
                case TipoNotificacionNegocio.SolicitudRechazada:
                case TipoNotificacionNegocio.DocumentacionIncompleta:
                    // Notificar al afiliado
                    destinatarios.Add(solicitud.AfiliadoSolicitante.Email);
                    break;

                case TipoNotificacionNegocio.NuevaSolicitudParaRevisar:
                    // Notificar a empleados (esto debería obtenerse de configuración o BD)
                    // Por ahora es solo un placeholder
                    break;
            }

            return destinatarios;
        }

        public string GenerarAsuntoEmail(TipoNotificacionNegocio tipo, SolicitudSubsidio solicitud)
        {
            return tipo switch
            {
                TipoNotificacionNegocio.SolicitudCreada =>
                    $"Solicitud {solicitud.NumeroSolicitud} - Creada exitosamente",

                TipoNotificacionNegocio.SolicitudEnviada =>
                    $"Solicitud {solicitud.NumeroSolicitud} - Enviada para revisión",

                TipoNotificacionNegocio.SolicitudAprobada =>
                    $"Solicitud {solicitud.NumeroSolicitud} - APROBADA",

                TipoNotificacionNegocio.SolicitudRechazada =>
                    $"Solicitud {solicitud.NumeroSolicitud} - Rechazada",

                TipoNotificacionNegocio.DocumentacionIncompleta =>
                    $"Solicitud {solicitud.NumeroSolicitud} - Documentación incompleta",

                TipoNotificacionNegocio.NuevaSolicitudParaRevisar =>
                    $"Nueva solicitud pendiente de revisión - {solicitud.NumeroSolicitud}",

                _ => $"Notificación - Solicitud {solicitud.NumeroSolicitud}"
            };
        }

        public Dictionary<string, object> GenerarDatosNotificacion(SolicitudSubsidio solicitud)
        {
            return new Dictionary<string, object>
            {
                { "NumeroSolicitud", solicitud.NumeroSolicitud },
                { "TipoSubsidio", solicitud.TipoSubsidio.ObtenerDescripcion() },
                { "FechaSolicitud", solicitud.FechaSolicitud.ToString("dd/MM/yyyy") },
                { "Estado", solicitud.Estado.ObtenerDescripcion() },
                { "NombreAfiliado", solicitud.AfiliadoSolicitante.NombreCompleto() },
                { "MatriculaProfesional", solicitud.AfiliadoSolicitante.MatriculaProfesional },
                { "EmailAfiliado", solicitud.AfiliadoSolicitante.Email }
            };
        }
    }
}
