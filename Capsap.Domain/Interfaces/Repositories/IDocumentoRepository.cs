using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Repositories
{
    // ==========================================
    // REPOSITORIO DE DOCUMENTOS
    // ==========================================
    public interface IDocumentoRepository : IRepositoryBase<DocumentoAdjunto>
    {
        Task<IEnumerable<DocumentoAdjunto>> GetBySolicitudIdAsync(int solicitudId);
        Task<DocumentoAdjunto> GetByIdWithSolicitudAsync(int id);
        Task<IEnumerable<DocumentoAdjunto>> GetPorTipoDocumentoAsync(int solicitudId, TipoDocumento tipo);
        Task<bool> ExisteDocumentoAsync(int solicitudId, TipoDocumento tipo);
        Task<IEnumerable<DocumentoAdjunto>> GetDocumentosNoCertificadosAsync(int solicitudId);
        Task<IEnumerable<DocumentoAdjunto>> GetDocumentosCertificadosAsync(int solicitudId);
        Task CertificarDocumentoAsync(int documentoId, int usuarioId);
        Task<long> ObtenerTamanoTotalDocumentosAsync(int solicitudId);
    }

}
