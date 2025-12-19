using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se encuentra una solicitud
    /// </summary>
    public class SolicitudNoEncontradaException : DomainException
    {
        public int? SolicitudId { get; }
        public string NumeroSolicitud { get; }

        public SolicitudNoEncontradaException(int solicitudId)
            : base(
                $"No se encontró la solicitud con ID {solicitudId}.",
                "SOLICITUD_NO_ENCONTRADA")
        {
            SolicitudId = solicitudId;
            AddData("SolicitudId", solicitudId);
        }

        public SolicitudNoEncontradaException(string numeroSolicitud)
            : base(
                $"No se encontró la solicitud con número {numeroSolicitud}.",
                "SOLICITUD_NO_ENCONTRADA")
        {
            NumeroSolicitud = numeroSolicitud;
            AddData("NumeroSolicitud", numeroSolicitud);
        }
    }

}
