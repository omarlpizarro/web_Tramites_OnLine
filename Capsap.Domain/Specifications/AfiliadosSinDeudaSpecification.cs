using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Specifications
{
    // Specification: Afiliados sin deuda
    public class AfiliadosSinDeudaSpecification : Specification<Afiliado>
    {
        public override Expression<Func<Afiliado, bool>> ToExpression()
        {
            return a => !a.TieneDeuda && a.Activo;
        }
    }

}
