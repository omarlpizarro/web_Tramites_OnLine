using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Interfaces.Repositories
{
    interface IUnitOfWork : IDisposable
    {
        IAfiliadoRepository Afiliados { get; }
        ISolicitudSubsidioRepository Solicitudes { get; }
        IDocumentoRepository Documentos { get; }
        IUsuarioRepository Usuarios { get; }
        IHistorialSolicitudRepository Historial { get; }
        IConfiguracionRepository Configuracion { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
