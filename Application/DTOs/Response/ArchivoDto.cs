using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    // ==========================================
    // DTO DE ARCHIVO DESCARGABLE
    // ==========================================

    public class ArchivoDto
    {
        public byte[] Contenido { get; set; }
        public string NombreArchivo { get; set; }
        public string ContentType { get; set; }
    }
}
