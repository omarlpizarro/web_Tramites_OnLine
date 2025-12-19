using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando una solicitud no puede ser modificada
    /// </summary>
    public class SolicitudNoModificableException : DomainException
    {
        public int SolicitudId { get; }
        public EstadoSolicitud EstadoActual { get; }

        public SolicitudNoModificableException(int solicitudId, EstadoSolicitud estadoActual)
            : base(
                $"La solicitud con ID {solicitudId} no puede ser modificada. Estado actual: {estadoActual}.",
                "SOLICITUD_NO_MODIFICABLE")
        {
            SolicitudId = solicitudId;
            EstadoActual = estadoActual;
            AddData("SolicitudId", solicitudId);
            AddData("EstadoActual", estadoActual);
        }
    }

}
