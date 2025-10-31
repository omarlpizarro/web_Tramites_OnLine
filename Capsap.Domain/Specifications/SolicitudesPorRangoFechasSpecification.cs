using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Specifications
{
    // Specification: Solicitudes por rango de fechas
    public class SolicitudesPorRangoFechasSpecification : Specification<SolicitudSubsidio>
    {
        private readonly DateTime _fechaDesde;
        private readonly DateTime _fechaHasta;

        public SolicitudesPorRangoFechasSpecification(DateTime fechaDesde, DateTime fechaHasta)
        {
            _fechaDesde = fechaDesde;
            _fechaHasta = fechaHasta;
        }

        public override Expression<Func<SolicitudSubsidio, bool>> ToExpression()
        {
            return s => s.FechaSolicitud >= _fechaDesde &&
                       s.FechaSolicitud <= _fechaHasta &&
                       s.Activo;
        }
    }

}
