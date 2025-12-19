using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    // ==========================================
    // DOCUMENTOS ADJUNTOS
    // ==========================================
    public class DocumentoAdjunto : EntityBase
    {
        public int SolicitudSubsidioId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public string RutaArchivo { get; set; }
        public string ContentType { get; set; }
        public long TamanoBytes { get; set; }
        public DateTime FechaCarga { get; set; }
        public bool Certificado { get; set; } // Si fue certificado por empleado
        public int? CertificadoPorUsuarioId { get; set; }
        public DateTime? FechaCertificacion { get; set; }

        public virtual SolicitudSubsidio Solicitud { get; set; }
        public virtual Usuario CertificadoPor { get; set; }
    }

}
