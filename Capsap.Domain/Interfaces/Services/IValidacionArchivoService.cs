using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Services
{
    // ==========================================
    // SERVICIO DE VALIDACIÓN DE ARCHIVOS
    // ==========================================
    public interface IValidacionArchivoService
    {
        Task<Result> ValidarArchivoAsync(Stream archivo, string nombreArchivo, long tamanoBytes);
        bool EsExtensionPermitida(string extension);
        bool EsTamanoPermitido(long tamanoBytes);
        Task<bool> EsArchivoSeguroAsync(Stream archivo);
        string ObtenerContentType(string extension);
    }
}
