using Application.DTOs.Response;
using Application.Interfaces;
using Capsap.Domain.Entities;
using Capsap.Domain.Extensions;
using Capsap.Domain.Interfaces.Repositories;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AfiliadoService : IAfiliadoService
    {
        private readonly IAfiliadoRepository _afiliadoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AfiliadoService(
            IAfiliadoRepository afiliadoRepository,
            IUnitOfWork unitOfWork)
        {
            _afiliadoRepository = afiliadoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AfiliadoDto>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var afiliado = await _afiliadoRepository.ObtenerPorIdAsync(id);
                if (afiliado == null)
                    return Result<AfiliadoDto>.Failure("Afiliado no encontrado");

                var dto = MapearAAfiliadoDto(afiliado);
                return Result<AfiliadoDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<AfiliadoDto>.Failure($"Error al obtener afiliado: {ex.Message}");
            }
        }

        public async Task<Result<AfiliadoDto>> ObtenerPorMatriculaAsync(string matricula)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(matricula))
                    return Result<AfiliadoDto>.Failure("La matrícula es requerida");

                var afiliado = await _afiliadoRepository.ObtenerPorMatriculaAsync(matricula);
                if (afiliado == null)
                    return Result<AfiliadoDto>.Failure($"No se encontró afiliado con matrícula {matricula}");

                var dto = MapearAAfiliadoDto(afiliado);
                return Result<AfiliadoDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<AfiliadoDto>.Failure($"Error al obtener afiliado: {ex.Message}");
            }
        }

        public async Task<Result<AfiliadoDto>> ObtenerPorDNIAsync(string dni)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dni))
                    return Result<AfiliadoDto>.Failure("El DNI es requerido");

                var afiliado = await _afiliadoRepository.ObtenerPorDNIAsync(dni);
                if (afiliado == null)
                    return Result<AfiliadoDto>.Failure($"No se encontró afiliado con DNI {dni}");

                var dto = MapearAAfiliadoDto(afiliado);
                return Result<AfiliadoDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<AfiliadoDto>.Failure($"Error al obtener afiliado: {ex.Message}");
            }
        }

        public async Task<Result<AfiliadoDto>> ObtenerPorUsuarioIdAsync(int usuarioId)
        {
            try
            {
                var afiliado = await _afiliadoRepository.ObtenerPorUsuarioIdAsync(usuarioId);
                if (afiliado == null)
                    return Result<AfiliadoDto>.Failure("No se encontró afiliado asociado al usuario");

                var dto = MapearAAfiliadoDto(afiliado);
                return Result<AfiliadoDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<AfiliadoDto>.Failure($"Error al obtener afiliado: {ex.Message}");
            }
        }

        public async Task<Result<List<AfiliadoDto>>> BuscarAfiliadosAsync(string criterio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(criterio))
                    return Result<List<AfiliadoDto>>.Failure("Debe proporcionar un criterio de búsqueda");

                var afiliados = await _afiliadoRepository.BuscarAsync(criterio);
                var dtos = afiliados.Select(a => MapearAAfiliadoDto(a)).ToList();

                return Result<List<AfiliadoDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return Result<List<AfiliadoDto>>.Failure($"Error al buscar afiliados: {ex.Message}");
            }
        }

        public async Task<Result<bool>> VerificarDeudaAsync(int afiliadoId)
        {
            try
            {
                var afiliado = await _afiliadoRepository.ObtenerPorIdAsync(afiliadoId);

                if (afiliado == null)
                    return Result<bool>.Failure("Afiliado no encontrado");

                return Result<bool>.Success(afiliado.TieneDeuda);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al verificar deuda: {ex.Message}");
            }
        }

        public async Task<Result> ActualizarDatosContactoAsync(int afiliadoId, string email, string telefono, string domicilio)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var afiliado = await _afiliadoRepository.ObtenerPorIdAsync(afiliadoId);
                if (afiliado == null)
                    return Result.Failure("Afiliado no encontrado");

                // Actualizar datos
                if (!string.IsNullOrWhiteSpace(email))
                    afiliado.Email = email;

                if (!string.IsNullOrWhiteSpace(telefono))
                    afiliado.Telefono = telefono;

                if (!string.IsNullOrWhiteSpace(domicilio))
                    afiliado.Domicilio = domicilio;

                afiliado.FechaModificacion = DateTime.Now;

                await _afiliadoRepository.ActualizarAsync(afiliado);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure($"Error al actualizar datos de contacto: {ex.Message}");
            }
        }

        private AfiliadoDto MapearAAfiliadoDto(Afiliado afiliado)
        {
            return new AfiliadoDto
            {
                Id = afiliado.Id,
                MatriculaProfesional = afiliado.MatriculaProfesional,
                Apellido = afiliado.Apellido,
                Nombre = afiliado.Nombre,
                NombreCompleto = afiliado.NombreCompleto(),
                DNI = afiliado.DNI,
                CUIL = afiliado.CUIL,
                EstadoCivil = afiliado.EstadoCivil,
                EstadoCivilDescripcion = afiliado.EstadoCivil.ObtenerDescripcion(),
                Email = afiliado.Email,
                Telefono = afiliado.Telefono,
                Domicilio = afiliado.Domicilio,
                Ciudad = afiliado.Ciudad,
                Provincia = afiliado.Provincia,
                TieneDeuda = afiliado.TieneDeuda,
                Activo = afiliado.Activo
            };
        }
    }
}
