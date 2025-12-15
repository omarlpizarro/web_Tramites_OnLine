using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAfiliadoRepository
    {
        Task<Afiliado?> ObtenerPorIdAsync(int id);
        Task<Afiliado?> ObtenerPorMatriculaAsync(string matricula);
        Task<Afiliado?> ObtenerPorDNIAsync(string dni);
        Task<Afiliado?> ObtenerPorCUILAsync(string cuil);
        Task<Afiliado?> ObtenerPorEmailAsync(string email);
        Task<Afiliado?> ObtenerPorUsuarioIdAsync(int usuarioId);
        Task<List<Afiliado>> BuscarAsync(string criterio);
        Task<List<Afiliado>> ObtenerTodosAsync();
        Task<List<Afiliado>> ObtenerConDeudasAsync();
        Task<bool> ExisteMatriculaAsync(string matricula, int? excluyendoId = null);
        Task<bool> ExisteDNIAsync(string dni, int? excluyendoId = null);
        Task<bool> ExisteCUILAsync(string cuil, int? excluyendoId = null);
        Task<Afiliado> AgregarAsync(Afiliado afiliado);
        Task ActualizarAsync(Afiliado afiliado);
        Task EliminarAsync(int id);
        Task<int> ContarActivosAsync();
        Task<int> ContarConDeudasAsync();
    }
}
