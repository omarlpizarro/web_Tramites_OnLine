using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    // ==========================================
    // EXCEPCIONES DE DOCUMENTACIÓN
    // ==========================================

    /// <summary>
    /// Excepción lanzada cuando la documentación está incompleta
    /// </summary>
    public class DocumentacionIncompletaException : DomainException
    {
        public List<string> DocumentosFaltantes { get; }
        public int SolicitudId { get; }

        public DocumentacionIncompletaException(List<string> documentosFaltantes)
            : base(
                $"Documentación incompleta. Faltan los siguientes documentos: {string.Join(", ", documentosFaltantes)}",
                "DOCUMENTACION_INCOMPLETA")
        {
            DocumentosFaltantes = documentosFaltantes ?? new List<string>();
            AddData("DocumentosFaltantes", documentosFaltantes);
        }

        public DocumentacionIncompletaException(int solicitudId, List<string> documentosFaltantes)
            : base(
                $"La solicitud {solicitudId} tiene documentación incompleta. Faltan: {string.Join(", ", documentosFaltantes)}",
                "DOCUMENTACION_INCOMPLETA")
        {
            SolicitudId = solicitudId;
            DocumentosFaltantes = documentosFaltantes ?? new List<string>();
            AddData("SolicitudId", solicitudId);
            AddData("DocumentosFaltantes", documentosFaltantes);
        }
    }

}
