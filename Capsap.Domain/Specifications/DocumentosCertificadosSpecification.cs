using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Specifications
{
    // Specification: Documentos certificados
    public class DocumentosCertificadosSpecification : Specification<DocumentoAdjunto>
    {
        public override Expression<Func<DocumentoAdjunto, bool>> ToExpression()
        {
            return d => d.Certificado && d.Activo;
        }
    }

}
