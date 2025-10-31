using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Events
{
    public class DocumentoCertificadoEvent : DomainEvent
    {
        public int DocumentoId { get; set; }
        public int SolicitudId { get; set; }
        public int CertificadoPorUsuarioId { get; set; }

        public DocumentoCertificadoEvent(int documentoId, int solicitudId, int certificadoPorUsuarioId)
        {
            DocumentoId = documentoId;
            SolicitudId = solicitudId;
            CertificadoPorUsuarioId = certificadoPorUsuarioId;
        }
    }

}
