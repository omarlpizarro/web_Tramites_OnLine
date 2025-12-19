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
    // ==========================================
    // SERVICIO DE WORKFLOW DE SOLICITUDES
    // ==========================================
    public interface IWorkflowSolicitudDomainService
    {
        Result PuedeTransicionarAEstado(SolicitudSubsidio solicitud, EstadoSolicitud nuevoEstado, RolUsuario rolUsuario);
        List<EstadoSolicitud> ObtenerEstadosPermitidos(EstadoSolicitud estadoActual, RolUsuario rolUsuario);
        Result ValidarTransicion(EstadoSolicitud estadoActual, EstadoSolicitud nuevoEstado);
    }

}
