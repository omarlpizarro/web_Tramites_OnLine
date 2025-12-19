using Application.Interfaces;
using Capsap.Domain.Entities;
using Capsap.Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Repositories
{
    public class AfiliadoRepository : IAfiliadoRepository
    {
        private readonly CapsapDbContext _context;

        public AfiliadoRepository(CapsapDbContext context)
        {
            _context = context;
        }

        public async Task<Afiliado?> ObtenerPorIdAsync(int id)
        {
            return await _context.Afiliados
                .Include(a => a.Usuario)
                .Include(a => a.Solicitudes)
                .FirstOrDefaultAsync(a => a.Id == id && a.Activo);
        }

        public async Task<Afiliado?> ObtenerPorMatriculaAsync(string matricula)
        {
            return await _context.Afiliados
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.MatriculaProfesional == matricula && a.Activo);
        }

        public async Task<Afiliado?> ObtenerPorDNIAsync(string dni)
        {
            return await _context.Afiliados
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.DNI == dni && a.Activo);
        }

        public async Task<Afiliado?> ObtenerPorCUILAsync(string cuil)
        {
            return await _context.Afiliados
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.CUIL == cuil && a.Activo);
        }

        public async Task<Afiliado?> ObtenerPorEmailAsync(string email)
        {
            return await _context.Afiliados
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Email == email && a.Activo);
        }

        public async Task<Afiliado?> ObtenerPorUsuarioIdAsync(int usuarioId)
        {
            return await _context.Afiliados
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Usuario.Id == usuarioId && a.Activo);
        }

        public async Task<List<Afiliado>> BuscarAsync(string criterio)
        {
            var criterioBusqueda = criterio.ToLower();

            return await _context.Afiliados
                .Include(a => a.Usuario)
                .Where(a => a.Activo &&
                    (a.Nombre.ToLower().Contains(criterioBusqueda) ||
                     a.Apellido.ToLower().Contains(criterioBusqueda) ||
                     a.DNI.Contains(criterioBusqueda) ||
                     a.MatriculaProfesional.Contains(criterioBusqueda) ||
                     a.CUIL.Contains(criterioBusqueda)))
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .Take(50) // Limitar resultados
                .ToListAsync();
        }

        public async Task<List<Afiliado>> ObtenerTodosAsync()
        {
            return await _context.Afiliados
                .Include(a => a.Usuario)
                .Where(a => a.Activo)
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .ToListAsync();
        }

        public async Task<List<Afiliado>> ObtenerConDeudasAsync()
        {
            return await _context.Afiliados
                .Include(a => a.Usuario)
                .Where(a => a.Activo && a.TieneDeuda)
                .OrderBy(a => a.Apellido)
                .ToListAsync();
        }

        public async Task<bool> ExisteMatriculaAsync(string matricula, int? excluyendoId = null)
        {
            var query = _context.Afiliados.Where(a => a.MatriculaProfesional == matricula);

            if (excluyendoId.HasValue)
            {
                query = query.Where(a => a.Id != excluyendoId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExisteDNIAsync(string dni, int? excluyendoId = null)
        {
            var query = _context.Afiliados.Where(a => a.DNI == dni);

            if (excluyendoId.HasValue)
            {
                query = query.Where(a => a.Id != excluyendoId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExisteCUILAsync(string cuil, int? excluyendoId = null)
        {
            var query = _context.Afiliados.Where(a => a.CUIL == cuil);

            if (excluyendoId.HasValue)
            {
                query = query.Where(a => a.Id != excluyendoId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<Afiliado> AgregarAsync(Afiliado afiliado)
        {
            await _context.Afiliados.AddAsync(afiliado);
            await _context.SaveChangesAsync();
            return afiliado;
        }

        public async Task ActualizarAsync(Afiliado afiliado)
        {
            _context.Afiliados.Update(afiliado);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var afiliado = await _context.Afiliados.FindAsync(id);
            if (afiliado != null)
            {
                afiliado.Activo = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> ContarActivosAsync()
        {
            return await _context.Afiliados.CountAsync(a => a.Activo);
        }

        public async Task<int> ContarConDeudasAsync()
        {
            return await _context.Afiliados.CountAsync(a => a.Activo && a.TieneDeuda);
        }
    }
}
