using Application.DTOs.Response;
using Application.Interfaces;
using Capsap.Domain.Enums;
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
    public class ReporteService : IReporteService
    {
        private readonly ISolicitudSubsidioRepository _solicitudRepository;

        public ReporteService(ISolicitudSubsidioRepository solicitudRepository)
        {
            _solicitudRepository = solicitudRepository;
        }

        public async Task<Result<EstadisticasGeneralesDto>> ObtenerEstadisticasGeneralesAsync()
        {
            try
            {
                var estadisticas = new EstadisticasGeneralesDto
                {
                    TotalSolicitudesMes = await _solicitudRepository.ContarSolicitudesDelMesAsync(),
                    SolicitudesPendientes = await _solicitudRepository.ContarPorEstadoAsync(EstadoSolicitud.Enviada) +
                                           await _solicitudRepository.ContarPorEstadoAsync(EstadoSolicitud.EnRevision),
                    SolicitudesAprobadas = await _solicitudRepository.ContarPorEstadoAsync(EstadoSolicitud.Aprobada),
                    SolicitudesRechazadas = await _solicitudRepository.ContarPorEstadoAsync(EstadoSolicitud.Rechazada),
                    TiempoPromedioResolucionDias = await _solicitudRepository.ObtenerTiempoPromedioResolucionAsync()
                };

                // Solicitudes por tipo
                estadisticas.SolicitudesPorTipo = new Dictionary<string, int>();
                foreach (TipoSubsidio tipo in Enum.GetValues(typeof(TipoSubsidio)))
                {
                    var cantidad = await _solicitudRepository.ContarPorTipoAsync(tipo);
                    estadisticas.SolicitudesPorTipo[tipo.ObtenerDescripcion()] = cantidad;
                }

                // Solicitudes por estado
                estadisticas.SolicitudesPorEstado = new Dictionary<string, int>();
                foreach (EstadoSolicitud estado in Enum.GetValues(typeof(EstadoSolicitud)))
                {
                    var cantidad = await _solicitudRepository.ContarPorEstadoAsync(estado);
                    estadisticas.SolicitudesPorEstado[estado.ObtenerDescripcion()] = cantidad;
                }

                // Últimas 10 solicitudes
                var ultimasSolicitudes = await _solicitudRepository.ObtenerTodasAsync();
                estadisticas.UltimasSolicitudes = ultimasSolicitudes
                    .OrderByDescending(s => s.FechaSolicitud)
                    .Take(10)
                    .Select(s => new SolicitudResponseDto
                    {
                        Id = s.Id,
                        NumeroSolicitud = s.NumeroSolicitud,
                        FechaSolicitud = s.FechaSolicitud,
                        Estado = s.Estado,
                        EstadoDescripcion = s.Estado.ObtenerDescripcion(),
                        TipoSubsidio = s.TipoSubsidio,
                        TipoSubsidioDescripcion = s.TipoSubsidio.ObtenerDescripcion(),
                        MatriculaAfiliado = s.AfiliadoSolicitante.MatriculaProfesional,
                        NombreCompletoAfiliado = s.AfiliadoSolicitante.NombreCompleto(),
                        DiasEnTramite = s.DiasEnTramite()
                    })
                    .ToList();

                return Result<EstadisticasGeneralesDto>.Success(estadisticas);
            }
            catch (Exception ex)
            {
                return Result<EstadisticasGeneralesDto>.Failure($"Error al obtener estadísticas: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GenerarReporteSolicitudesAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            TipoSubsidio? tipo = null)
        {
            try
            {
                var solicitudes = await _solicitudRepository.GetPorRangoFechasAsync(fechaDesde, fechaHasta);

                // Convertir 'solicitudes' a una lista explícitamente para evitar el error CS0266
                //solicitudes = solicitudes.Where(s => s.TipoSubsidio == tipo.Value).ToList();
                if (tipo.HasValue)
                {
                    solicitudes = solicitudes.Where(s => s.TipoSubsidio == tipo.Value).ToList();
                }

                // Aquí implementarías la generación del reporte en Excel o PDF
                // Por ahora retornamos un placeholder
                var contenido = System.Text.Encoding.UTF8.GetBytes("Reporte de solicitudes");

                return Result<byte[]>.Success(contenido);
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failure($"Error al generar reporte: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GenerarReporteAfiliadosAsync()
        {
            try
            {
                // Implementar generación de reporte de afiliados
                var contenido = System.Text.Encoding.UTF8.GetBytes("Reporte de afiliados");
                return Result<byte[]>.Success(contenido);
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failure($"Error al generar reporte: {ex.Message}");
            }
        }

        public async Task<Result<Dictionary<string, int>>> ObtenerSolicitudesPorMesAsync(int anio)
        {
            try
            {
                var resultado = new Dictionary<string, int>();
                var meses = new[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

                for (int mes = 1; mes <= 12; mes++)
                {
                    var fechaDesde = new DateTime(anio, mes, 1);
                    var fechaHasta = fechaDesde.AddMonths(1).AddDays(-1);

                    var solicitudes = await _solicitudRepository.GetPorRangoFechasAsync(fechaDesde, fechaHasta);
                    resultado[meses[mes - 1]] = solicitudes.Count();
                }

                return Result<Dictionary<string, int>>.Success(resultado);
            }
            catch (Exception ex)
            {
                return Result<Dictionary<string, int>>.Failure($"Error al obtener solicitudes por mes: {ex.Message}");
            }
        }

        public async Task<Result<Dictionary<TipoSubsidio, int>>> ObtenerSolicitudesPorTipoAsync(
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            try
            {
                var resultado = new Dictionary<TipoSubsidio, int>();

                foreach (TipoSubsidio tipo in Enum.GetValues(typeof(TipoSubsidio)))
                {
                    var cantidad = await _solicitudRepository.ContarPorTipoAsync(tipo);
                    resultado[tipo] = cantidad;
                }

                return Result<Dictionary<TipoSubsidio, int>>.Success(resultado);
            }
            catch (Exception ex)
            {
                return Result<Dictionary<TipoSubsidio, int>>.Failure($"Error al obtener solicitudes por tipo: {ex.Message}");
            }
        }

        public async Task<Result<Dictionary<EstadoSolicitud, int>>> ObtenerSolicitudesPorEstadoAsync()
        {
            try
            {
                var resultado = new Dictionary<EstadoSolicitud, int>();

                foreach (EstadoSolicitud estado in Enum.GetValues(typeof(EstadoSolicitud)))
                {
                    var cantidad = await _solicitudRepository.ContarPorEstadoAsync(estado);
                    resultado[estado] = cantidad;
                }

                return Result<Dictionary<EstadoSolicitud, int>>.Success(resultado);
            }
            catch (Exception ex)
            {
                return Result<Dictionary<EstadoSolicitud, int>>.Failure($"Error al obtener solicitudes por estado: {ex.Message}");
            }
        }
    }
}
