using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando la extensión del archivo no está permitida
    /// </summary>
    public class ExtensionNoPermitidaException : DomainException
    {
        public string NombreArchivo { get; }
        public string Extension { get; }
        public string[] ExtensionesPermitidas { get; }

        public ExtensionNoPermitidaException(string nombreArchivo, string extension, string[] extensionesPermitidas)
            : base(
                $"La extensión '{extension}' del archivo '{nombreArchivo}' no está permitida. Extensiones permitidas: {string.Join(", ", extensionesPermitidas)}",
                "EXTENSION_NO_PERMITIDA")
        {
            NombreArchivo = nombreArchivo;
            Extension = extension;
            ExtensionesPermitidas = extensionesPermitidas;
            AddData("NombreArchivo", nombreArchivo);
            AddData("Extension", extension);
            AddData("ExtensionesPermitidas", extensionesPermitidas);
        }
    }

}
