using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Specifications
{
    // Specification: Solicitudes de un afiliado
    public class SolicitudesPorAfiliadoSpecification : Specification<SolicitudSubsidio>
    {
        private readonly int _afiliadoId;

        public SolicitudesPorAfiliadoSpecification(int afiliadoId)
        {
            _afiliadoId = afiliadoId;
        }

        public override Expression<Func<SolicitudSubsidio, bool>> ToExpression()
        {
            return s => s.AfiliadoSolicitanteId == _afiliadoId && s.Activo;
        }
    }

}
