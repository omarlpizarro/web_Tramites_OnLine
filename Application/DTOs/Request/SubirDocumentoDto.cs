using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    // ==========================================
    // DTOs DE REQUEST - SUBIR DOCUMENTOS
    // ==========================================

    public class SubirDocumentoDto
    {
        public int SolicitudId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public byte[] Contenido { get; set; }
        public string ContentType { get; set; }
        public long TamanoBytes { get; set; }
    }
}
