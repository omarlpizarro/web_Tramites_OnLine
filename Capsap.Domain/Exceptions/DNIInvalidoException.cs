using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando un DNI no es válido
    /// </summary>
    public class DNIInvalidoException : DomainException
    {
        public string DNI { get; }

        public DNIInvalidoException(string dni)
            : base(
                $"El DNI '{dni}' no es válido. Debe tener entre 7 y 8 dígitos numéricos.",
                "DNI_INVALIDO")
        {
            DNI = dni;
            AddData("DNI", dni);
        }

        public DNIInvalidoException(string dni, string razon)
            : base(
                $"El DNI '{dni}' no es válido. {razon}",
                "DNI_INVALIDO")
        {
            DNI = dni;
            AddData("DNI", dni);
            AddData("Razon", razon);
        }
    }

}
