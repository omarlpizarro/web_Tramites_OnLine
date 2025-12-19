using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando un documento no es válido
    /// </summary>
    public class DocumentoInvalidoException : DomainException
    {
        public string NombreArchivo { get; }
        public string Razon { get; }

        public DocumentoInvalidoException(string nombreArchivo, string razon)
            : base(
                $"El documento '{nombreArchivo}' no es válido. {razon}",
                "DOCUMENTO_INVALIDO")
        {
            NombreArchivo = nombreArchivo;
            Razon = razon;
            AddData("NombreArchivo", nombreArchivo);
            AddData("Razon", razon);
        }
    }

}
