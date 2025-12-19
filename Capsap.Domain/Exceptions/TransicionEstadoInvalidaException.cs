using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    // ==========================================
    // EXCEPCIONES DE WORKFLOW
    // ==========================================

    /// <summary>
    /// Excepción lanzada cuando una transición de estado no es permitida
    /// </summary>
    public class TransicionEstadoInvalidaException : DomainException
    {
        public EstadoSolicitud EstadoActual { get; }
        public EstadoSolicitud EstadoDestino { get; }

        public TransicionEstadoInvalidaException(EstadoSolicitud estadoActual, EstadoSolicitud estadoDestino)
            : base(
                $"No se puede transicionar del estado '{estadoActual}' al estado '{estadoDestino}'.",
                "TRANSICION_ESTADO_INVALIDA")
        {
            EstadoActual = estadoActual;
            EstadoDestino = estadoDestino;
            AddData("EstadoActual", estadoActual);
            AddData("EstadoDestino", estadoDestino);
        }
    }

}
