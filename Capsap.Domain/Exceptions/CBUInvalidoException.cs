using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    // ==========================================
    // EXCEPCIONES DE VALIDACIÓN
    // ==========================================

    /// <summary>
    /// Excepción lanzada cuando un CBU no es válido
    /// </summary>
    public class CBUInvalidoException : DomainException
    {
        public string CBU { get; }

        public CBUInvalidoException(string cbu)
            : base(
                $"El CBU '{cbu}' no es válido. Debe tener 22 dígitos numéricos.",
                "CBU_INVALIDO")
        {
            CBU = cbu;
            AddData("CBU", cbu);
        }

        public CBUInvalidoException(string cbu, string razon)
            : base(
                $"El CBU '{cbu}' no es válido. {razon}",
                "CBU_INVALIDO")
        {
            CBU = cbu;
            AddData("CBU", cbu);
            AddData("Razon", razon);
        }
    }

}
