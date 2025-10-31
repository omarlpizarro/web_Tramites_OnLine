using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Services
{
    // ==========================================
    // SERVICIO DE AUDITORÍA
    // ==========================================
    public interface IAuditoriaService
    {
        Task RegistrarAccionAsync(int usuarioId, string accion, string entidad, int entidadId, string detalles = null);
        Task<IEnumerable<RegistroAuditoria>> ObtenerAuditoriaPorUsuarioAsync(int usuarioId, DateTime? fechaDesde = null, DateTime? fechaHasta = null);
        Task<IEnumerable<RegistroAuditoria>> ObtenerAuditoriaPorEntidadAsync(string entidad, int entidadId);
        Task<IEnumerable<RegistroAuditoria>> ObtenerAuditoriaRecienteAsync(int cantidad = 50);
    }
}
