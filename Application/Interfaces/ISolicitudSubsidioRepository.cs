using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // ==========================================
    // ISolicitudSubsidioRepository
    // ==========================================
    public interface ISolicitudSubsidioRepository
    {
        Task<SolicitudSubsidio?> ObtenerPorIdAsync(int id);
        Task<SolicitudSubsidio?> ObtenerPorNumeroSolicitudAsync(string numeroSolicitud);
        Task<List<SolicitudSubsidio>> ObtenerPorAfiliadoAsync(int afiliadoId);
        Task<List<SolicitudSubsidio>> ObtenerPorEstadoAsync(EstadoSolicitud estado);
        Task<List<SolicitudSubsidio>> ObtenerPendientesAsync();
        Task<List<SolicitudSubsidio>> ObtenerTodasAsync();
        Task<List<SolicitudSubsidio>> BuscarAsync(
            EstadoSolicitud? estado = null,
            TipoSubsidio? tipoSubsidio = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            string? criterioBusqueda = null,
            int? afiliadoId = null);
        Task<SolicitudSubsidio> AgregarAsync(SolicitudSubsidio solicitud);
        Task ActualizarAsync(SolicitudSubsidio solicitud);
        Task EliminarAsync(int id);
        Task<string> GenerarNumeroSolicitudAsync(TipoSubsidio tipoSubsidio);
        Task<bool> ExisteNumeroSolicitudAsync(string numeroSolicitud);
        Task<int> ContarPorEstadoAsync(EstadoSolicitud estado);
        Task<int> ContarPorTipoAsync(TipoSubsidio tipo);
        Task<Dictionary<EstadoSolicitud, int>> ObtenerEstadisticasPorEstadoAsync();
        Task AgregarHistorialAsync(HistorialSolicitud historial);
    }
}
