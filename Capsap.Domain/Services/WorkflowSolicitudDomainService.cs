using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    public class WorkflowSolicitudDomainService : IWorkflowSolicitudDomainService
    {
        public Result PuedeTransicionarAEstado(SolicitudSubsidio solicitud, EstadoSolicitud nuevoEstado, RolUsuario rolUsuario)
        {
            // Validar que la transición sea válida según el estado actual
            var validacionTransicion = ValidarTransicion(solicitud.Estado, nuevoEstado);
            if (!validacionTransicion.IsSuccess)
            {
                return validacionTransicion;
            }

            // Validar permisos según rol
            var estadosPermitidos = ObtenerEstadosPermitidos(solicitud.Estado, rolUsuario);
            if (!estadosPermitidos.Contains(nuevoEstado))
            {
                return Result.Failure($"El rol {rolUsuario} no tiene permisos para cambiar la solicitud al estado {nuevoEstado}");
            }

            return Result.Success();
        }

        public List<EstadoSolicitud> ObtenerEstadosPermitidos(EstadoSolicitud estadoActual, RolUsuario rolUsuario)
        {
            var estadosPermitidos = new List<EstadoSolicitud>();

            switch (rolUsuario)
            {
                case RolUsuario.Afiliado:
                    if (estadoActual == EstadoSolicitud.Borrador)
                    {
                        estadosPermitidos.Add(EstadoSolicitud.Enviada);
                    }
                    break;

                case RolUsuario.EmpleadoMesaEntrada:
                    if (estadoActual == EstadoSolicitud.Enviada)
                    {
                        estadosPermitidos.Add(EstadoSolicitud.EnRevision);
                        estadosPermitidos.Add(EstadoSolicitud.DocumentacionIncompleta);
                    }
                    if (estadoActual == EstadoSolicitud.DocumentacionIncompleta)
                    {
                        estadosPermitidos.Add(EstadoSolicitud.EnRevision);
                    }
                    break;

                case RolUsuario.EmpleadoRevisor:
                    if (estadoActual == EstadoSolicitud.Enviada)
                    {
                        estadosPermitidos.Add(EstadoSolicitud.EnRevision);
                        estadosPermitidos.Add(EstadoSolicitud.DocumentacionIncompleta);
                    }
                    if (estadoActual == EstadoSolicitud.EnRevision)
                    {
                        estadosPermitidos.Add(EstadoSolicitud.Aprobada);
                        estadosPermitidos.Add(EstadoSolicitud.Rechazada);
                        estadosPermitidos.Add(EstadoSolicitud.DocumentacionIncompleta);
                    }
                    if (estadoActual == EstadoSolicitud.Aprobada)
                    {
                        estadosPermitidos.Add(EstadoSolicitud.Pagada);
                    }
                    break;

                case RolUsuario.Administrador:
                    // El administrador puede transicionar a cualquier estado
                    estadosPermitidos.AddRange(Enum.GetValues<EstadoSolicitud>());
                    break;
            }

            return estadosPermitidos;
        }

        public Result ValidarTransicion(EstadoSolicitud estadoActual, EstadoSolicitud nuevoEstado)
        {
            // Definir transiciones válidas
            var transicionesValidas = new Dictionary<EstadoSolicitud, List<EstadoSolicitud>>
            {
                { EstadoSolicitud.Borrador, new List<EstadoSolicitud> { EstadoSolicitud.Enviada } },
                { EstadoSolicitud.Enviada, new List<EstadoSolicitud> { EstadoSolicitud.EnRevision, EstadoSolicitud.DocumentacionIncompleta, EstadoSolicitud.Rechazada } },
                { EstadoSolicitud.EnRevision, new List<EstadoSolicitud> { EstadoSolicitud.Aprobada, EstadoSolicitud.Rechazada, EstadoSolicitud.DocumentacionIncompleta } },
                { EstadoSolicitud.DocumentacionIncompleta, new List<EstadoSolicitud> { EstadoSolicitud.EnRevision, EstadoSolicitud.Rechazada } },
                { EstadoSolicitud.Aprobada, new List<EstadoSolicitud> { EstadoSolicitud.Pagada } },
                { EstadoSolicitud.Rechazada, new List<EstadoSolicitud>() }, // Estado terminal
                { EstadoSolicitud.Pagada, new List<EstadoSolicitud>() } // Estado terminal
            };

            if (!transicionesValidas.ContainsKey(estadoActual))
            {
                return Result.Failure($"Estado actual {estadoActual} no es válido");
            }

            if (!transicionesValidas[estadoActual].Contains(nuevoEstado))
            {
                return Result.Failure($"No se puede transicionar de {estadoActual} a {nuevoEstado}");
            }

            return Result.Success();
        }
    }

}
