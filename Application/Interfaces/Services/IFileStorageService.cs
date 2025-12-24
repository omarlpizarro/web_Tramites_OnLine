using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    // ==========================================
    // IFileStorageService
    // ==========================================
    public interface IFileStorageService
    {
        Task<string> GuardarArchivoAsync(IFormFile archivo, string carpeta);
        Task<byte[]> ObtenerArchivoAsync(string rutaArchivo);
        Task<bool> EliminarArchivoAsync(string rutaArchivo);
        Task<bool> ExisteArchivoAsync(string rutaArchivo);
        Task<string> MoverArchivoAsync(string rutaOrigen, string carpetaDestino);
        Task<string> CopiarArchivoAsync(string rutaOrigen, string carpetaDestino);
        Task<long> ObtenerTamanoArchivoAsync(string rutaArchivo);
        Task<string> ObtenerRutaCompletaAsync(string rutaRelativa);
        bool ValidarArchivo(IFormFile archivo);
        bool ValidarTipoMime(string contentType);
        Task LimpiarArchivosTemporalesAsync(int diasAntiguedad = 30);
        Task<Dictionary<string, long>> ObtenerEstadisticasAlmacenamientoAsync();
        // Métodos específicos para documentos
        Task<string> GuardarDocumentoSolicitudAsync(int solicitudId, Stream archivo, string nombreArchivo);
        Task<byte[]> ObtenerDocumentoSolicitudAsync(int solicitudId, string nombreArchivo);
        Task LimpiarArchivosTemporalesAsync();

    }
}
