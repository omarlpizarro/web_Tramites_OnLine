using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Specifications
{
    // ==========================================
    // SPECIFICATIONS ESPECÍFICAS
    // ==========================================

    // Specification: Solicitudes pendientes
    public class SolicitudesPendientesSpecification : Specification<SolicitudSubsidio>
    {
        public override Expression<Func<SolicitudSubsidio, bool>> ToExpression()
        {
            return s => s.Estado == EstadoSolicitud.Enviada ||
                       s.Estado == EstadoSolicitud.EnRevision ||
                       s.Estado == EstadoSolicitud.DocumentacionIncompleta;
        }
    }

}
