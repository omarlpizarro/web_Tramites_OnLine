using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class DetalleSolicitudDto : SolicitudResponseDto
    {
        public string Observaciones { get; set; }
        public string ObservacionesInternas { get; set; }
        public DatosBancariosDto DatosBancarios { get; set; }
        public List<DocumentoDto> Documentos { get; set; }
        public List<HistorialCambioDto> Historial { get; set; }
        public object DetalleEspecifico { get; set; } // Puede ser Matrimonio, Maternidad, etc.
    }

}
