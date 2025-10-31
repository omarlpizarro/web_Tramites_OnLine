using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Services
{
    // ==========================================
    // SERVICIO DE ALMACENAMIENTO DE ARCHIVOS
    // ==========================================
    public interface IFileStorageService
    {
        Task<string> GuardarArchivoAsync(Stream archivoStream, string nombreArchivo, string carpeta = null);
        Task<byte[]> ObtenerArchivoAsync(string rutaArchivo);
        Task<Stream> ObtenerArchivoStreamAsync(string rutaArchivo);
        Task<bool> EliminarArchivoAsync(string rutaArchivo);
        Task<bool> ExisteArchivoAsync(string rutaArchivo);
        Task<long> ObtenerTamanoArchivoAsync(string rutaArchivo);
        Task<string> CopiarArchivoAsync(string rutaOrigen, string rutaDestino);
        Task<string> MoverArchivoAsync(string rutaOrigen, string rutaDestino);

        // Métodos específicos para documentos
        Task<string> GuardarDocumentoSolicitudAsync(int solicitudId, Stream archivo, string nombreArchivo);
        Task<byte[]> ObtenerDocumentoSolicitudAsync(int solicitudId, string nombreArchivo);
        Task LimpiarArchivosTemporalesAsync();
    }
}
