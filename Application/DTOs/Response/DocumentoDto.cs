using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class DocumentoDto
    {
        public int Id { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string TipoDocumentoDescripcion { get; set; }
        public string NombreArchivo { get; set; }
        public string ContentType { get; set; }
        public long TamanoBytes { get; set; }
        public string TamanoLegible { get; set; }
        public DateTime FechaCarga { get; set; }
        public bool Certificado { get; set; }
        public DateTime? FechaCertificacion { get; set; }
        public string CertificadoPor { get; set; }
    }

}
