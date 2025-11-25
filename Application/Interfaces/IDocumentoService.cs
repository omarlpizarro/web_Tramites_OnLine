using Application.DTOs.Response;
using Capsap.Domain.Enums;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Application.Interfaces
{
    // ==========================================
    // SERVICIO DE DOCUMENTOS
    // ==========================================
    public interface IDocumentoService
    {
        Task<Result<DocumentoDto>> SubirDocumentoAsync(int solicitudId, TipoDocumento tipoDocumento, IFormFile archivo, int usuarioId);
        Task<Result<ArchivoDto>> ObtenerDocumentoAsync(int documentoId, int usuarioId);
        Task<Result> CertificarDocumentoAsync(int documentoId, int usuarioId);
        Task<Result> EliminarDocumentoAsync(int documentoId, int usuarioId);
        Task<Result<List<DocumentoDto>>> ObtenerDocumentosSolicitudAsync(int solicitudId);
    }
}
