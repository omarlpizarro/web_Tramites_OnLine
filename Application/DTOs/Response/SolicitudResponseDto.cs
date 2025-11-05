using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    // ==========================================
    // DTOs DE RESPONSE
    // ==========================================
    public class SolicitudResponseDto
    {
        public int Id { get; set; }
        public string NumeroSolicitud { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public EstadoSolicitud Estado { get; set; }
        public string EstadoDescripcion { get; set; }
        public TipoSubsidio TipoSubsidio { get; set; }
        public string TipoSubsidioDescripcion { get; set; }
        public string MatriculaAfiliado { get; set; }
        public string NombreCompletoAfiliado { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public int DiasEnTramite { get; set; }
        public bool PuedeEditar { get; set; }
        public bool PuedeEnviar { get; set; }
        public bool PuedeCancelar { get; set; }
    }
}
