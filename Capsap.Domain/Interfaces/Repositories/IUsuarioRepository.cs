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
    // REPOSITORIO DE USUARIOS
    // ==========================================
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Task<Usuario> GetByEmailAsync(string email);
        Task<Usuario> GetByIdWithAfiliadoAsync(int id);
        Task<IEnumerable<Usuario>> GetByRolAsync(RolUsuario rol);
        Task<bool> EmailExisteAsync(string email);
        Task<Usuario> ValidarCredencialesAsync(string email, string password);
    }

}
