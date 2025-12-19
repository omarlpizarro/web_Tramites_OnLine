using Application.DTOs.Response;
using Capsap.Domain.Enums;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // ==========================================
    // SERVICIO DE REPORTES
    // ==========================================
    public interface IReporteService
    {
        Task<Result<EstadisticasGeneralesDto>> ObtenerEstadisticasGeneralesAsync();
        Task<Result<byte[]>> GenerarReporteSolicitudesAsync(System.DateTime fechaDesde, System.DateTime fechaHasta, TipoSubsidio? tipo = null);
        Task<Result<byte[]>> GenerarReporteAfiliadosAsync();
        Task<Result<Dictionary<string, int>>> ObtenerSolicitudesPorMesAsync(int anio);
        Task<Result<Dictionary<TipoSubsidio, int>>> ObtenerSolicitudesPorTipoAsync(System.DateTime? fechaDesde = null, System.DateTime? fechaHasta = null);
        Task<Result<Dictionary<EstadoSolicitud, int>>> ObtenerSolicitudesPorEstadoAsync();
    }
}
