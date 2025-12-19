using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando un CUIL/CUIT no es válido
    /// </summary>
    public class CUILInvalidoException : DomainException
    {
        public string CUIL { get; }

        public CUILInvalidoException(string cuil)
            : base(
                $"El CUIL/CUIT '{cuil}' no es válido. Debe tener 11 dígitos numéricos y un dígito verificador correcto.",
                "CUIL_INVALIDO")
        {
            CUIL = cuil;
            AddData("CUIL", cuil);
        }

        public CUILInvalidoException(string cuil, string razon)
            : base(
                $"El CUIL/CUIT '{cuil}' no es válido. {razon}",
                "CUIL_INVALIDO")
        {
            CUIL = cuil;
            AddData("CUIL", cuil);
            AddData("Razon", razon);
        }
    }

}
