using Application.DTOs.Response;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // ==========================================
    // SERVICIO DE AFILIADOS
    // ==========================================
    public interface IAfiliadoService
    {
        Task<Result<AfiliadoDto>> ObtenerPorIdAsync(int id);
        Task<Result<AfiliadoDto>> ObtenerPorMatriculaAsync(string matricula);
        Task<Result<AfiliadoDto>> ObtenerPorDNIAsync(string dni);
        Task<Result<AfiliadoDto>> ObtenerPorUsuarioIdAsync(int usuarioId);
        Task<Result<List<AfiliadoDto>>> BuscarAfiliadosAsync(string criterio);
        Task<Result<bool>> VerificarDeudaAsync(int afiliadoId);
        Task<Result> ActualizarDatosContactoAsync(int afiliadoId, string email, string telefono, string domicilio);
    }
}
