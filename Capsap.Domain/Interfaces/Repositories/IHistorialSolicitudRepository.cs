using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Repositories
{
    // ==========================================
    // REPOSITORIO DE HISTORIAL
    // ==========================================
    public interface IHistorialSolicitudRepository : IRepositoryBase<HistorialSolicitud>
    {
        Task<IEnumerable<HistorialSolicitud>> GetBySolicitudIdAsync(int solicitudId);
        Task<IEnumerable<HistorialSolicitud>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<HistorialSolicitud>> GetCambiosRecientesAsync(int cantidad = 10);
    }

}
