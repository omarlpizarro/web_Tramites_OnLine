using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Repositories
{
    // ==========================================
    // REPOSITORIO DE SOLICITUDES DE SUBSIDIO
    // ==========================================
    public interface ISolicitudSubsidioRepository : IRepositoryBase<SolicitudSubsidio>
    {
        Task<SolicitudSubsidio> GetByIdWithDetailsAsync(int id);
        Task<SolicitudSubsidio> GetByNumeroSolicitudAsync(string numeroSolicitud);
        Task<IEnumerable<SolicitudSubsidio>> GetByAfiliadoIdAsync(int afiliadoId);
        Task<IEnumerable<SolicitudSubsidio>> GetByEstadoAsync(EstadoSolicitud estado);
        Task<IEnumerable<SolicitudSubsidio>> GetPendientesAsync();
        Task<IEnumerable<SolicitudSubsidio>> GetEnRevisionAsync();
        Task<IEnumerable<SolicitudSubsidio>> GetAprobadasAsync();
        Task<IEnumerable<SolicitudSubsidio>> GetPorTipoSubsidioAsync(TipoSubsidio tipo);
        Task<IEnumerable<SolicitudSubsidio>> GetPorRangoFechasAsync(DateTime fechaDesde, DateTime fechaHasta);
        Task<int> ObtenerSiguienteCorrelativoAsync(string periodo, string prefijo);
        Task<IEnumerable<SolicitudSubsidio>> BuscarAsync(string numeroSolicitud, string matricula, EstadoSolicitud? estado, TipoSubsidio? tipo);

        // Estadísticas
        Task<int> ContarSolicitudesPorEstadoAsync(EstadoSolicitud estado);
        Task<int> ContarSolicitudesPorTipoAsync(TipoSubsidio tipo);
        Task<int> ContarSolicitudesDelMesAsync();
        Task<double> ObtenerTiempoPromedioResolucionAsync();
    }

}
