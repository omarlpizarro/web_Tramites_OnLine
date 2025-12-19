using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Repositories
{
    // ==========================================
    // REPOSITORIO DE CONFIGURACIÓN
    // ==========================================
    public interface IConfiguracionRepository : IRepositoryBase<ConfiguracionSistema>
    {
        Task<ConfiguracionSistema> GetByClaveAsync(string clave);
        Task<string> ObtenerValorAsync(string clave, string valorPorDefecto = null);
        Task<int> ObtenerValorEnteroAsync(string clave, int valorPorDefecto = 0);
        Task<decimal> ObtenerValorDecimalAsync(string clave, decimal valorPorDefecto = 0);
        Task<bool> ObtenerValorBoolAsync(string clave, bool valorPorDefecto = false);
        Task ActualizarValorAsync(string clave, string valor);
    }

}
