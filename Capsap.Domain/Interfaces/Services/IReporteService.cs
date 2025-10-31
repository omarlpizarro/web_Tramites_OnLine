using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Services
{
    // ==========================================
    // SERVICIO DE REPORTES
    // ==========================================
    public interface IReporteService
    {
        Task<EstadisticasGeneralesDto> ObtenerEstadisticasGeneralesAsync();
        Task<byte[]> GenerarReporteSolicitudesAsync(DateTime fechaDesde, DateTime fechaHasta, TipoSubsidio? tipo = null);
        Task<byte[]> GenerarReporteAfiliadosAsync();
        Task<Dictionary<string, int>> ObtenerSolicitudesPorMesAsync(int anio);
        Task<Dictionary<TipoSubsidio, int>> ObtenerSolicitudesPorTipoAsync(DateTime? fechaDesde = null, DateTime? fechaHasta = null);
        Task<Dictionary<EstadoSolicitud, int>> ObtenerSolicitudesPorEstadoAsync();
    }
}
