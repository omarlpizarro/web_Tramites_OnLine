using Application.DTOs.Request;
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
    // SERVICIO DE SOLICITUDES
    // ==========================================
    public interface ISolicitudSubsidioService
    {
        // Crear solicitudes
        Task<Result<SolicitudResponseDto>> CrearSolicitudMatrimonioAsync(CrearSolicitudMatrimonioDto dto);
        Task<Result<SolicitudResponseDto>> CrearSolicitudMaternidadAsync(CrearSolicitudMaternidadDto dto);
        Task<Result<SolicitudResponseDto>> CrearSolicitudNacimientoAsync(CrearSolicitudNacimientoDto dto);
        Task<Result<SolicitudResponseDto>> CrearSolicitudDiscapacidadAsync(CrearSolicitudDiscapacidadDto dto);

        // Acciones sobre solicitudes
        Task<Result> EnviarSolicitudAsync(int solicitudId, int usuarioId);
        Task<Result> AprobarSolicitudAsync(int solicitudId, int usuarioId, string comentario);
        Task<Result> RechazarSolicitudAsync(int solicitudId, int usuarioId, string motivo);
        Task<Result> SolicitarDocumentacionAdicionalAsync(int solicitudId, int usuarioId, List<string> documentosFaltantes);
        Task<Result> CancelarSolicitudAsync(int solicitudId, int usuarioId);

        // Consultas
        Task<Result<DetalleSolicitudDto>> ObtenerDetalleSolicitudAsync(int solicitudId, int usuarioId);
        Task<Result<List<SolicitudResponseDto>>> ObtenerSolicitudesAfiliadoAsync(int afiliadoId);
        Task<Result<List<SolicitudResponseDto>>> ObtenerSolicitudesPendientesAsync();
        Task<Result<PaginatedListDto<SolicitudResponseDto>>> BuscarSolicitudesAsync(BuscarSolicitudesDto filtros);
        Task<Result<List<SolicitudResponseDto>>> ObtenerTodasLasSolicitudesAsync();
    }
}
