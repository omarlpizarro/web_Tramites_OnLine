using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    / ==========================================
// IPdfGeneratorService
// ==========================================
public interface IPdfGeneratorService
    {
        Task<byte[]> GenerarComprobanteSolicitudAsync(SolicitudSubsidio solicitud);
        Task<byte[]> GenerarResolucionAsync(SolicitudSubsidio solicitud, string resolucion);
        Task<byte[]> GenerarReporteSolicitudesAsync(List<SolicitudSubsidio> solicitudes, DateTime? fechaDesde, DateTime? fechaHasta);
        Task<byte[]> GenerarConstanciaDeudaAsync(Afiliado afiliado, bool tieneDeuda, decimal? montoDeuda = null);
    }
}
