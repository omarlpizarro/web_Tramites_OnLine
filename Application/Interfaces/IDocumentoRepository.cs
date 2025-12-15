using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // ==========================================
    // IDocumentoRepository
    // ==========================================
    public interface IDocumentoRepository
    {
        Task<DocumentoAdjunto?> ObtenerPorIdAsync(int id);
        Task<List<DocumentoAdjunto>> ObtenerPorSolicitudAsync(int solicitudId);
        Task<List<DocumentoAdjunto>> ObtenerPorTipoAsync(int solicitudId, TipoDocumento tipoDocumento);
        Task<List<DocumentoAdjunto>> ObtenerCertificadosAsync(int solicitudId);
        Task<List<DocumentoAdjunto>> ObtenerNoCertificadosAsync(int solicitudId);
        Task<DocumentoAdjunto?> ObtenerPorRutaAsync(string rutaArchivo);
        Task<bool> ExisteDocumentoTipoAsync(int solicitudId, TipoDocumento tipoDocumento);
        Task<DocumentoAdjunto> AgregarAsync(DocumentoAdjunto documento);
        Task ActualizarAsync(DocumentoAdjunto documento);
        Task EliminarAsync(int id);
        Task<int> ContarPorSolicitudAsync(int solicitudId);
        Task<int> ContarCertificadosAsync(int solicitudId);
        Task<long> ObtenerTamanoTotalAsync(int solicitudId);
        Task<List<DocumentoAdjunto>> ObtenerDocumentosRecientesAsync(int cantidad = 10);
        Task<bool> TieneTodosDocumentosRequeridosAsync(int solicitudId, TipoSubsidio tipoSubsidio);
    }
}
