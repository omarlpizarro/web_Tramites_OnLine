using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Repositories
{
    // ==========================================
    // REPOSITORIO DE AFILIADOS
    // ==========================================
    public interface IAfiliadoRepository : IRepositoryBase<Afiliado>
    {
        Task<Afiliado> GetByMatriculaAsync(string matriculaProfesional);
        Task<Afiliado> GetByDNIAsync(string dni);
        Task<Afiliado> GetByEmailAsync(string email);
        Task<Afiliado> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Afiliado>> GetAfiliadosActivosAsync();
        Task<IEnumerable<Afiliado>> GetAfiliadosConDeudaAsync();
        Task<bool> TieneDeudaAsync(int afiliadoId);
        Task ActualizarEstadoDeudaAsync(int afiliadoId, bool tieneDeuda);
        Task<IEnumerable<Afiliado>> BuscarAsync(string criterio);
    }

}
