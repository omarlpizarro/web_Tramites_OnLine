using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando el tamaño del archivo excede el límite
    /// </summary>
    public class ArchivoMuyGrandeException : DomainException
    {
        public string NombreArchivo { get; }
        public long TamanoBytes { get; }
        public long TamanoMaximoBytes { get; }

        public ArchivoMuyGrandeException(string nombreArchivo, long tamanoBytes, long tamanoMaximoBytes)
            : base(
                $"El archivo '{nombreArchivo}' excede el tamaño máximo permitido. Tamaño: {tamanoBytes / 1024 / 1024} MB, Máximo: {tamanoMaximoBytes / 1024 / 1024} MB.",
                "ARCHIVO_MUY_GRANDE")
        {
            NombreArchivo = nombreArchivo;
            TamanoBytes = tamanoBytes;
            TamanoMaximoBytes = tamanoMaximoBytes;
            AddData("NombreArchivo", nombreArchivo);
            AddData("TamanoBytes", tamanoBytes);
            AddData("TamanoMaximoBytes", tamanoMaximoBytes);
        }
    }

}
