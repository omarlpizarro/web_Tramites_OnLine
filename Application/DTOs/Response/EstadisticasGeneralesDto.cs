using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    // ==========================================
    // DTOs DE ESTADÍSTICAS
    // ==========================================

    public class EstadisticasGeneralesDto
    {
        public int TotalSolicitudesMes { get; set; }
        public int SolicitudesPendientes { get; set; }
        public int SolicitudesAprobadas { get; set; }
        public int SolicitudesRechazadas { get; set; }
        public double TiempoPromedioResolucionDias { get; set; }
        public Dictionary<string, int> SolicitudesPorTipo { get; set; }
        public Dictionary<string, int> SolicitudesPorEstado { get; set; }
        public List<SolicitudResponseDto> UltimasSolicitudes { get; set; }
    }
}
