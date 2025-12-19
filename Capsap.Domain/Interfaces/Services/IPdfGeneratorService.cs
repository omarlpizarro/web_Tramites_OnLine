using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Services
{
    // ==========================================
    // SERVICIO DE GENERACIÓN DE PDF
    // ==========================================
    public interface IPdfGeneratorService
    {
        Task<byte[]> GenerarComprobanteSolicitudAsync(SolicitudSubsidio solicitud);
        Task<byte[]> GenerarFormularioPrecompletoAsync(SolicitudSubsidio solicitud);
        Task<byte[]> GenerarReporteAsync(string tipoReporte, Dictionary<string, object> parametros);
        Task<byte[]> GenerarConstanciaAprobacionAsync(SolicitudSubsidio solicitud);
    }
}
