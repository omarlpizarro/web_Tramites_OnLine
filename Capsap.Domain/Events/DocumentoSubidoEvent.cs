using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Events
{
    public class DocumentoSubidoEvent : DomainEvent
    {
        public int DocumentoId { get; set; }
        public int SolicitudId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public int SubidoPorUsuarioId { get; set; }

        public DocumentoSubidoEvent(int documentoId, int solicitudId, TipoDocumento tipoDocumento, string nombreArchivo, int subidoPorUsuarioId)
        {
            DocumentoId = documentoId;
            SolicitudId = solicitudId;
            TipoDocumento = tipoDocumento;
            NombreArchivo = nombreArchivo;
            SubidoPorUsuarioId = subidoPorUsuarioId;
        }
    }

}
