using Application.DTOs;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using Capsap.Domain.Extensions;
using Capsap.Domain.Interfaces.Repositories;
using Capsap.Domain.Interfaces.Services;
using Capsap.Domain.Services;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    // ==========================================
    // IMPLEMENTACIÓN DEL SERVICIO
    // ==========================================
    public class SolicitudSubsidioService : ISolicitudSubsidioService
    {
        private readonly ISolicitudSubsidioRepository _solicitudRepository;
        private readonly IAfiliadoRepository _afiliadoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmailService _emailService;
        private readonly IValidacionService _validacionService;
        private readonly IGeneradorNumeroSolicitudDomainService _generadorNumero;
        private readonly IWorkflowSolicitudDomainService _workflowService;
        private readonly IValidacionDocumentacionDomainService _validacionDocumentacion;
        private readonly IUnitOfWork _unitOfWork;

        public SolicitudSubsidioService(
            ISolicitudSubsidioRepository solicitudRepository,
            IAfiliadoRepository afiliadoRepository,
            IUsuarioRepository usuarioRepository,
            IEmailService emailService,
            IValidacionService validacionService,
            IGeneradorNumeroSolicitudDomainService generadorNumero,
            IWorkflowSolicitudDomainService workflowService,
            IValidacionDocumentacionDomainService validacionDocumentacion,
            IUnitOfWork unitOfWork)
        {
            _solicitudRepository = solicitudRepository;
            _afiliadoRepository = afiliadoRepository;
            _usuarioRepository = usuarioRepository;
            _emailService = emailService;
            _validacionService = validacionService;
            _generadorNumero = generadorNumero;
            _workflowService = workflowService;
            _validacionDocumentacion = validacionDocumentacion;
            _unitOfWork = unitOfWork;
        }

        // ==========================================
        // CREAR SOLICITUD DE MATRIMONIO
        // ==========================================
        public async Task<Result<SolicitudResponseDto>> CrearSolicitudMatrimonioAsync(CrearSolicitudMatrimonioDto dto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // 1. Obtener afiliado
                var afiliado = await _afiliadoRepository.GetByIdAsync(dto.AfiliadoSolicitanteId);
                if (afiliado == null)
                    return Result<SolicitudResponseDto>.Failure("Afiliado no encontrado");

                // 2. Verificar deuda
                var verificacionDeuda = await _validacionService.VerificarDeudaAsync(afiliado.Id);
                if (!verificacionDeuda.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(verificacionDeuda.Error);

                // 3. Validar plazo
                var validacionPlazo = await _validacionService.ValidarPlazoSolicitudAsync(
                    TipoSubsidio.Matrimonio,
                    dto.FechaCelebracion);
                if (!validacionPlazo.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(validacionPlazo.Error);

                // 4. Crear solicitud principal
                var resultSolicitud = SolicitudSubsidio.Crear(
                    afiliado,
                    TipoSubsidio.Matrimonio,
                    dto.CBU,
                    dto.TipoCuenta,
                    dto.Banco);

                if (!resultSolicitud.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(resultSolicitud.Error);

                var solicitud = resultSolicitud.Value;
                solicitud.Observaciones = dto.Observaciones;

                // 5. Agregar datos específicos de matrimonio
                var resultMatrimonio = SubsidioMatrimonio.Crear(
                    solicitud,
                    dto.FechaCelebracion,
                    dto.ActaNumero,
                    dto.Tomo,
                    dto.Anio,
                    dto.Ciudad,
                    dto.Provincia,
                    dto.DNIConyuge,
                    dto.CUILConyuge,
                    dto.AmbosAfiliadosActivos);

                if (!resultMatrimonio.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(resultMatrimonio.Error);

                solicitud.SubsidioMatrimonio = resultMatrimonio.Value;

                // 6. Configurar transferencia a tercero si corresponde
                if (dto.TransferenciaATercero)
                {
                    solicitud.TransferenciaATercero = true;
                    solicitud.TitularCuenta = dto.TitularCuenta;
                    solicitud.CUITTitular = dto.CUITTitular;
                }

                // 7. Generar número de solicitud
                var periodo = _generadorNumero.ObtenerPeriodo();
                var prefijo = _generadorNumero.ObtenerPrefijo(TipoSubsidio.Matrimonio);
                var correlativo = await _solicitudRepository.ObtenerSiguienteCorrelativoAsync(periodo, prefijo);
                solicitud.NumeroSolicitud = _generadorNumero.GenerarNumeroSolicitud(TipoSubsidio.Matrimonio, correlativo);

                // 8. Guardar en base de datos
                await _solicitudRepository.AddAsync(solicitud);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // 9. Enviar email de confirmación
                await _emailService.EnviarEmailSolicitudCreadaAsync(solicitud);

                // 10. Mapear a DTO de respuesta
                var response = MapearASolicitudResponseDto(solicitud);

                return Result<SolicitudResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<SolicitudResponseDto>.Failure($"Error al crear la solicitud: {ex.Message}");
            }
        }

        // ==========================================
        // CREAR SOLICITUD DE MATERNIDAD
        // ==========================================
        public async Task<Result<SolicitudResponseDto>> CrearSolicitudMaternidadAsync(CrearSolicitudMaternidadDto dto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var afiliado = await _afiliadoRepository.GetByIdAsync(dto.AfiliadoSolicitanteId);
                if (afiliado == null)
                    return Result<SolicitudResponseDto>.Failure("Afiliado no encontrado");

                var verificacionDeuda = await _validacionService.VerificarDeudaAsync(afiliado.Id);
                if (!verificacionDeuda.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(verificacionDeuda.Error);

                var validacionPlazo = await _validacionService.ValidarPlazoSolicitudAsync(
                    TipoSubsidio.Maternidad,
                    dto.FechaEvento);
                if (!validacionPlazo.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(validacionPlazo.Error);

                var resultSolicitud = SolicitudSubsidio.Crear(
                    afiliado,
                    TipoSubsidio.Maternidad,
                    dto.CBU,
                    dto.TipoCuenta,
                    dto.Banco);

                if (!resultSolicitud.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(resultSolicitud.Error);

                var solicitud = resultSolicitud.Value;
                solicitud.Observaciones = dto.Observaciones;

                // Crear subsidio maternidad
                solicitud.SubsidioMaternidad = new SubsidioMaternidad
                {
                    TipoEvento = dto.TipoEvento,
                    FechaEvento = dto.FechaEvento,
                    ActaNumero = dto.ActaNumero,
                    Tomo = dto.Tomo,
                    Anio = dto.Anio,
                    Ciudad = dto.Ciudad,
                    Provincia = dto.Provincia,
                    NombreHijo = dto.NombreHijo,
                    DNIHijo = dto.DNIHijo,
                    CUILHijo = dto.CUILHijo,
                    FechaSentenciaJudicial = dto.FechaSentenciaJudicial,
                    CiudadSentencia = dto.CiudadSentencia,
                    ProvinciaSentencia = dto.ProvinciaSentencia,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                };

                var periodo = _generadorNumero.ObtenerPeriodo();
                var prefijo = _generadorNumero.ObtenerPrefijo(TipoSubsidio.Maternidad);
                var correlativo = await _solicitudRepository.ObtenerSiguienteCorrelativoAsync(periodo, prefijo);
                solicitud.NumeroSolicitud = _generadorNumero.GenerarNumeroSolicitud(TipoSubsidio.Maternidad, correlativo);

                await _solicitudRepository.AddAsync(solicitud);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                await _emailService.EnviarEmailSolicitudCreadaAsync(solicitud);

                var response = MapearASolicitudResponseDto(solicitud);
                return Result<SolicitudResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<SolicitudResponseDto>.Failure($"Error al crear la solicitud: {ex.Message}");
            }
        }

        // ==========================================
        // CREAR SOLICITUD DE NACIMIENTO/ADOPCIÓN
        // ==========================================
        public async Task<Result<SolicitudResponseDto>> CrearSolicitudNacimientoAsync(CrearSolicitudNacimientoDto dto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var afiliado = await _afiliadoRepository.GetByIdAsync(dto.AfiliadoSolicitanteId);
                if (afiliado == null)
                    return Result<SolicitudResponseDto>.Failure("Afiliado no encontrado");

                if (!dto.Hijos.Any())
                    return Result<SolicitudResponseDto>.Failure("Debe especificar al menos un hijo");

                var verificacionDeuda = await _validacionService.VerificarDeudaAsync(afiliado.Id);
                if (!verificacionDeuda.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(verificacionDeuda.Error);

                // Validar plazo con la fecha del primer hijo
                var fechaPrimerHijo = dto.Hijos.First().FechaSentenciaJudicial ?? dto.Hijos.First().FechaNacimiento;
                var validacionPlazo = await _validacionService.ValidarPlazoSolicitudAsync(
                    TipoSubsidio.NacimientoAdopcion,
                    fechaPrimerHijo);
                if (!validacionPlazo.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(validacionPlazo.Error);

                var resultSolicitud = SolicitudSubsidio.Crear(
                    afiliado,
                    TipoSubsidio.NacimientoAdopcion,
                    dto.CBU,
                    dto.TipoCuenta,
                    dto.Banco);

                if (!resultSolicitud.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(resultSolicitud.Error);

                var solicitud = resultSolicitud.Value;
                solicitud.Observaciones = dto.Observaciones;
                solicitud.AfiliadoConyuge2Id = dto.AfiliadoConyuge2Id;

                // Crear subsidio nacimiento/adopción
                var subsidioNacimiento = new SubsidioNacimientoAdopcion
                {
                    TipoEvento = dto.TipoEvento,
                    FechaCreacion = DateTime.Now,
                    Activo = true,
                    Hijos = dto.Hijos.Select(h => new HijoNacimientoAdopcion
                    {
                        Nombre = h.Nombre,
                        FechaNacimiento = h.FechaNacimiento,
                        ActaNumero = h.ActaNumero,
                        Tomo = h.Tomo,
                        Anio = h.Anio,
                        CiudadNacimiento = h.CiudadNacimiento,
                        ProvinciaNacimiento = h.ProvinciaNacimiento,
                        DNI = h.DNI,
                        CUIL = h.CUIL,
                        FechaSentenciaJudicial = h.FechaSentenciaJudicial,
                        CiudadSentencia = h.CiudadSentencia,
                        ProvinciaSentencia = h.ProvinciaSentencia,
                        FechaCreacion = DateTime.Now,
                        Activo = true
                    }).ToList()
                };

                solicitud.SubsidioNacimientoAdopcion = subsidioNacimiento;

                var periodo = _generadorNumero.ObtenerPeriodo();
                var prefijo = _generadorNumero.ObtenerPrefijo(TipoSubsidio.NacimientoAdopcion);
                var correlativo = await _solicitudRepository.ObtenerSiguienteCorrelativoAsync(periodo, prefijo);
                solicitud.NumeroSolicitud = _generadorNumero.GenerarNumeroSolicitud(TipoSubsidio.NacimientoAdopcion, correlativo);

                await _solicitudRepository.AddAsync(solicitud);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                await _emailService.EnviarEmailSolicitudCreadaAsync(solicitud);

                var response = MapearASolicitudResponseDto(solicitud);
                return Result<SolicitudResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<SolicitudResponseDto>.Failure($"Error al crear la solicitud: {ex.Message}");
            }
        }

        // ==========================================
        // CREAR SOLICITUD DE HIJO DISCAPACITADO
        // ==========================================
        public async Task<Result<SolicitudResponseDto>> CrearSolicitudDiscapacidadAsync(CrearSolicitudDiscapacidadDto dto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var afiliado = await _afiliadoRepository.GetByIdAsync(dto.AfiliadoSolicitanteId);
                if (afiliado == null)
                    return Result<SolicitudResponseDto>.Failure("Afiliado no encontrado");

                var verificacionDeuda = await _validacionService.VerificarDeudaAsync(afiliado.Id);
                if (!verificacionDeuda.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(verificacionDeuda.Error);

                var resultSolicitud = SolicitudSubsidio.Crear(
                    afiliado,
                    TipoSubsidio.HijoDiscapacitado,
                    dto.CBU,
                    dto.TipoCuenta,
                    dto.Banco);

                if (!resultSolicitud.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(resultSolicitud.Error);

                var solicitud = resultSolicitud.Value;
                solicitud.Observaciones = dto.Observaciones;

                var resultDiscapacidad = SubsidioHijoDiscapacitado.Crear(
                    solicitud,
                    dto.TipoSolicitud,
                    dto.NombreHijo,
                    dto.FechaNacimiento,
                    dto.DNI,
                    dto.NumeroCertificadoDiscapacidad,
                    dto.Diagnostico,
                    dto.FechaEmisionCertificado,
                    dto.FechaVencimientoCertificado,
                    dto.LugarEmision);

                if (!resultDiscapacidad.IsSuccess)
                    return Result<SolicitudResponseDto>.Failure(resultDiscapacidad.Error);

                solicitud.SubsidioHijoDiscapacitado = resultDiscapacidad.Value;
                solicitud.SubsidioHijoDiscapacitado.CUIL = dto.CUIL;
                solicitud.SubsidioHijoDiscapacitado.ActaNumero = dto.ActaNumero;
                solicitud.SubsidioHijoDiscapacitado.Tomo = dto.Tomo;
                solicitud.SubsidioHijoDiscapacitado.Anio = dto.Anio;
                solicitud.SubsidioHijoDiscapacitado.CiudadNacimiento = dto.CiudadNacimiento;
                solicitud.SubsidioHijoDiscapacitado.ProvinciaNacimiento = dto.ProvinciaNacimiento;

                var periodo = _generadorNumero.ObtenerPeriodo();
                var prefijo = _generadorNumero.ObtenerPrefijo(TipoSubsidio.HijoDiscapacitado);
                var correlativo = await _solicitudRepository.ObtenerSiguienteCorrelativoAsync(periodo, prefijo);
                solicitud.NumeroSolicitud = _generadorNumero.GenerarNumeroSolicitud(TipoSubsidio.HijoDiscapacitado, correlativo);

                await _solicitudRepository.AddAsync(solicitud);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                await _emailService.EnviarEmailSolicitudCreadaAsync(solicitud);

                var response = MapearASolicitudResponseDto(solicitud);
                return Result<SolicitudResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<SolicitudResponseDto>.Failure($"Error al crear la solicitud: {ex.Message}");
            }
        }

        // ==========================================
        // MÉTODOS AUXILIARES DE MAPEO
        // ==========================================
        private SolicitudResponseDto MapearASolicitudResponseDto(SolicitudSubsidio solicitud)
        {
            return new SolicitudResponseDto
            {
                Id = solicitud.Id,
                NumeroSolicitud = solicitud.NumeroSolicitud,
                FechaSolicitud = solicitud.FechaSolicitud,
                Estado = solicitud.Estado,
                EstadoDescripcion = solicitud.Estado.ObtenerDescripcion(),
                TipoSubsidio = solicitud.TipoSubsidio,
                TipoSubsidioDescripcion = solicitud.TipoSubsidio.ObtenerDescripcion(),
                MatriculaAfiliado = solicitud.AfiliadoSolicitante.MatriculaProfesional,
                NombreCompletoAfiliado = solicitud.AfiliadoSolicitante.NombreCompleto(),
                FechaResolucion = solicitud.FechaResolucion,
                DiasEnTramite = solicitud.DiasEnTramite(),
                PuedeEditar = solicitud.PuedeSerEditada(),
                PuedeEnviar = solicitud.PuedeSerEnviada(),
                PuedeCancelar = solicitud.Estado == EstadoSolicitud.Borrador || solicitud.Estado == EstadoSolicitud.Enviada
            };
        }

        // ==========================================
        // ENVIAR SOLICITUD
        // ==========================================
        public async Task<Result> EnviarSolicitudAsync(int solicitudId, int usuarioId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var solicitud = await _solicitudRepository.GetByIdWithDetailsAsync(solicitudId);
                if (solicitud == null)
                    return Result.Failure("Solicitud no encontrada");

                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                    return Result.Failure("Usuario no encontrado");

                // Verificar que el usuario es el propietario
                if (solicitud.AfiliadoSolicitanteId != usuario.AfiliadoId)
                    return Result.Failure("No tiene permisos para enviar esta solicitud");

                // Verificar que tiene todos los documentos requeridos
                var validacionDocumentos = _validacionDocumentacion.ValidarDocumentacionCompleta(solicitud);
                if (!validacionDocumentos.IsSuccess)
                    return validacionDocumentos;

                // Enviar la solicitud (ejecuta validaciones en el dominio)
                var result = solicitud.Enviar();
                if (!result.IsSuccess)
                    return result;

                await _solicitudRepository.UpdateAsync(solicitud);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Notificar a empleados
                await _emailService.EnviarEmailNuevaSolicitudAEmpleadosAsync(solicitud);
                await _emailService.EnviarEmailSolicitudEnviadaAsync(solicitud);

                return Result.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure($"Error al enviar la solicitud: {ex.Message}");
            }
        }

        // ==========================================
        // APROBAR SOLICITUD
        // ==========================================
        public async Task<Result> AprobarSolicitudAsync(int solicitudId, int usuarioId, string comentario)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var solicitud = await _solicitudRepository.GetByIdWithDetailsAsync(solicitudId);
                if (solicitud == null)
                    return Result.Failure("Solicitud no encontrada");

                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                    return Result.Failure("Usuario no encontrado");

                // Verificar permisos
                if (usuario.Rol != RolUsuario.EmpleadoRevisor && usuario.Rol != RolUsuario.Administrador)
                    return Result.Failure("No tiene permisos para aprobar solicitudes");

                // Validar transición de estado
                var validacionTransicion = _workflowService.PuedeTransicionarAEstado(
                    solicitud,
                    EstadoSolicitud.Aprobada,
                    usuario.Rol);

                if (!validacionTransicion.IsSuccess)
                    return validacionTransicion;

                // Aprobar solicitud (ejecuta validaciones en el dominio)
                var result = solicitud.Aprobar(usuario, comentario);
                if (!result.IsSuccess)
                    return result;

                await _solicitudRepository.UpdateAsync(solicitud);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Notificar al afiliado
                await _emailService.EnviarEmailSolicitudAprobadaAsync(solicitud);

                return Result.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure($"Error al aprobar la solicitud: {ex.Message}");
            }
        }

        // ==========================================
        // RECHAZAR SOLICITUD
        // ==========================================
        public async Task<Result> RechazarSolicitudAsync(int solicitudId, int usuarioId, string motivo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(motivo))
                    return Result.Failure("Debe proporcionar un motivo para el rechazo");

                await _unitOfWork.BeginTransactionAsync();

                var solicitud = await _solicitudRepository.GetByIdWithDetailsAsync(solicitudId);
                if (solicitud == null)
                    return Result.Failure("Solicitud no encontrada");

                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                    return Result.Failure("Usuario no encontrado");

                // Verificar permisos
                if (usuario.Rol != RolUsuario.EmpleadoRevisor && usuario.Rol != RolUsuario.Administrador)
                    return Result.Failure("No tiene permisos para rechazar solicitudes");

                // Rechazar solicitud
                var result = solicitud.Rechazar(usuario, motivo);
                if (!result.IsSuccess)
                    return result;

                await _solicitudRepository.UpdateAsync(solicitud);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Notificar al afiliado
                await _emailService.EnviarEmailSolicitudRechazadaAsync(solicitud, motivo);

                return Result.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure($"Error al rechazar la solicitud: {ex.Message}");
            }
        }

        // ==========================================
        // SOLICITAR DOCUMENTACIÓN ADICIONAL
        // ==========================================
        public async Task<Result> SolicitarDocumentacionAdicionalAsync(int solicitudId, int usuarioId, List<string> documentosFaltantes)
        {
            try
            {
                if (documentosFaltantes == null || !documentosFaltantes.Any())
                    return Result.Failure("Debe especificar los documentos faltantes");

                await _unitOfWork.BeginTransactionAsync();

                var solicitud = await _solicitudRepository.GetByIdWithDetailsAsync(solicitudId);
                if (solicitud == null)
                    return Result.Failure("Solicitud no encontrada");

                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                    return Result.Failure("Usuario no encontrado");

                // Verificar permisos
                if (usuario.Rol == RolUsuario.Afiliado)
                    return Result.Failure("No tiene permisos para realizar esta acción");

                // Cambiar estado a DocumentacionIncompleta
                solicitud.Estado = EstadoSolicitud.DocumentacionIncompleta;
                solicitud.ObservacionesInternas = $"Documentos faltantes: {string.Join(", ", documentosFaltantes)}";

                await _solicitudRepository.UpdateAsync(solicitud);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Notificar al afiliado
                await _emailService.EnviarEmailDocumentacionIncompletaAsync(solicitud, documentosFaltantes);

                return Result.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure($"Error al solicitar documentación: {ex.Message}");
            }
        }

        // ==========================================
        // CANCELAR SOLICITUD
        // ==========================================
        public async Task<Result> CancelarSolicitudAsync(int solicitudId, int usuarioId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var solicitud = await _solicitudRepository.GetByIdWithDetailsAsync(solicitudId);
                if (solicitud == null)
                    return Result.Failure("Solicitud no encontrada");

                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                    return Result.Failure("Usuario no encontrado");

                // Solo el afiliado propietario puede cancelar
                if (solicitud.AfiliadoSolicitanteId != usuario.AfiliadoId)
                    return Result.Failure("No tiene permisos para cancelar esta solicitud");

                // Solo se puede cancelar en ciertos estados
                if (!solicitud.PuedeSerEditada() && solicitud.Estado != EstadoSolicitud.Enviada)
                    return Result.Failure("La solicitud no puede ser cancelada en su estado actual");

                // Marcar como inactiva
                solicitud.Activo = false;
                solicitud.FechaModificacion = DateTime.Now;

                await _solicitudRepository.UpdateAsync(solicitud);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure($"Error al cancelar la solicitud: {ex.Message}");
            }
        }

        // ==========================================
        // OBTENER DETALLE DE SOLICITUD
        // ==========================================
        public async Task<Result<DetalleSolicitudDto>> ObtenerDetalleSolicitudAsync(int solicitudId, int usuarioId)
        {
            try
            {
                var solicitud = await _solicitudRepository.GetByIdWithDetailsAsync(solicitudId);
                if (solicitud == null)
                    return Result<DetalleSolicitudDto>.Failure("Solicitud no encontrada");

                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                    return Result<DetalleSolicitudDto>.Failure("Usuario no encontrado");

                // Verificar permisos
                if (usuario.Rol == RolUsuario.Afiliado && solicitud.AfiliadoSolicitanteId != usuario.AfiliadoId)
                    return Result<DetalleSolicitudDto>.Failure("No tiene permisos para ver esta solicitud");

                var detalle = MapearADetalleSolicitudDto(solicitud);
                return Result<DetalleSolicitudDto>.Success(detalle);
            }
            catch (Exception ex)
            {
                return Result<DetalleSolicitudDto>.Failure($"Error al obtener el detalle: {ex.Message}");
            }
        }

        // ==========================================
        // OBTENER SOLICITUDES DE UN AFILIADO
        // ==========================================
        public async Task<Result<List<SolicitudResponseDto>>> ObtenerSolicitudesAfiliadoAsync(int afiliadoId)
        {
            try
            {
                var solicitudes = await _solicitudRepository.GetByAfiliadoIdAsync(afiliadoId);
                var response = solicitudes.Select(s => MapearASolicitudResponseDto(s)).ToList();
                return Result<List<SolicitudResponseDto>>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<List<SolicitudResponseDto>>.Failure($"Error al obtener solicitudes: {ex.Message}");
            }
        }

        // ==========================================
        // OBTENER SOLICITUDES PENDIENTES
        // ==========================================
        public async Task<Result<List<SolicitudResponseDto>>> ObtenerSolicitudesPendientesAsync()
        {
            try
            {
                var solicitudes = await _solicitudRepository.GetPendientesAsync();
                var response = solicitudes.Select(s => MapearASolicitudResponseDto(s)).ToList();
                return Result<List<SolicitudResponseDto>>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<List<SolicitudResponseDto>>.Failure($"Error al obtener solicitudes pendientes: {ex.Message}");
            }
        }

        // ==========================================
        // OBTENER TODAS LAS SOLICITUDES
        // ==========================================
        public async Task<Result<List<SolicitudResponseDto>>> ObtenerTodasLasSolicitudesAsync()
        {
            try
            {
                var solicitudes = await _solicitudRepository.GetAllAsync();
                var response = solicitudes.Select(s => MapearASolicitudResponseDto(s)).ToList();
                return Result<List<SolicitudResponseDto>>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<List<SolicitudResponseDto>>.Failure($"Error al obtener solicitudes: {ex.Message}");
            }
        }

        // ==========================================
        // BUSCAR SOLICITUDES CON FILTROS
        // ==========================================
        public async Task<Result<PaginatedListDto<SolicitudResponseDto>>> BuscarSolicitudesAsync(BuscarSolicitudesDto filtros)
        {
            try
            {
                var solicitudes = await _solicitudRepository.BuscarAsync(
                    filtros.NumeroSolicitud,
                    filtros.MatriculaProfesional,
                    filtros.Estado,
                    filtros.TipoSubsidio);

                // Aplicar filtro de fechas si existe
                if (filtros.FechaDesde.HasValue)
                {
                    solicitudes = solicitudes.Where(s => s.FechaSolicitud >= filtros.FechaDesde.Value);
                }
                if (filtros.FechaHasta.HasValue)
                {
                    solicitudes = solicitudes.Where(s => s.FechaSolicitud <= filtros.FechaHasta.Value);
                }

                var totalRegistros = solicitudes.Count();

                // Paginación
                var solicitudesPaginadas = solicitudes
                    .Skip((filtros.PaginaActual - 1) * filtros.RegistrosPorPagina)
                    .Take(filtros.RegistrosPorPagina)
                    .Select(s => MapearASolicitudResponseDto(s))
                    .ToList();

                var resultado = new PaginatedListDto<SolicitudResponseDto>(
                    solicitudesPaginadas,
                    totalRegistros,
                    filtros.PaginaActual,
                    filtros.RegistrosPorPagina);

                return Result<PaginatedListDto<SolicitudResponseDto>>.Success(resultado);
            }
            catch (Exception ex)
            {
                return Result<PaginatedListDto<SolicitudResponseDto>>.Failure($"Error al buscar solicitudes: {ex.Message}");
            }
        }

        // ==========================================
        // MÉTODOS AUXILIARES DE MAPEO
        // ==========================================
        private SolicitudResponseDto MapearASolicitudResponseDto(SolicitudSubsidio solicitud)
        {
            return new SolicitudResponseDto
            {
                Id = solicitud.Id,
                NumeroSolicitud = solicitud.NumeroSolicitud,
                FechaSolicitud = solicitud.FechaSolicitud,
                Estado = solicitud.Estado,
                EstadoDescripcion = solicitud.Estado.ObtenerDescripcion(),
                TipoSubsidio = solicitud.TipoSubsidio,
                TipoSubsidioDescripcion = solicitud.TipoSubsidio.ObtenerDescripcion(),
                MatriculaAfiliado = solicitud.AfiliadoSolicitante.MatriculaProfesional,
                NombreCompletoAfiliado = solicitud.AfiliadoSolicitante.NombreCompleto(),
                FechaResolucion = solicitud.FechaResolucion,
                DiasEnTramite = solicitud.DiasEnTramite(),
                PuedeEditar = solicitud.PuedeSerEditada(),
                PuedeEnviar = solicitud.PuedeSerEnviada(),
                PuedeCancelar = solicitud.Estado == EstadoSolicitud.Borrador || solicitud.Estado == EstadoSolicitud.Enviada
            };
        }

        private DetalleSolicitudDto MapearADetalleSolicitudDto(SolicitudSubsidio solicitud)
        {
            var detalle = new DetalleSolicitudDto
            {
                Id = solicitud.Id,
                NumeroSolicitud = solicitud.NumeroSolicitud,
                FechaSolicitud = solicitud.FechaSolicitud,
                Estado = solicitud.Estado,
                EstadoDescripcion = solicitud.Estado.ObtenerDescripcion(),
                TipoSubsidio = solicitud.TipoSubsidio,
                TipoSubsidioDescripcion = solicitud.TipoSubsidio.ObtenerDescripcion(),
                MatriculaAfiliado = solicitud.AfiliadoSolicitante.MatriculaProfesional,
                NombreCompletoAfiliado = solicitud.AfiliadoSolicitante.NombreCompleto(),
                FechaResolucion = solicitud.FechaResolucion,
                DiasEnTramite = solicitud.DiasEnTramite(),
                Observaciones = solicitud.Observaciones,
                ObservacionesInternas = solicitud.ObservacionesInternas,
                PorcentajeCompletitud = solicitud.PorcentajeCompletitud(),
                DatosBancarios = new DatosBancariosDto
                {
                    CBU = solicitud.CBU,
                    CBUFormateado = solicitud.CBU.FormatearCBU(),
                    TipoCuenta = solicitud.TipoCuenta,
                    Banco = solicitud.Banco,
                    TransferenciaATercero = solicitud.TransferenciaATercero,
                    TitularCuenta = solicitud.TitularCuenta,
                    CUITTitular = solicitud.CUITTitular
                },
                Documentos = solicitud.Documentos.Where(d => d.Activo).Select(d => new DocumentoDto
                {
                    Id = d.Id,
                    TipoDocumento = d.TipoDocumento,
                    TipoDocumentoDescripcion = d.TipoDocumento.ObtenerDescripcion(),
                    NombreArchivo = d.NombreArchivo,
                    ContentType = d.ContentType,
                    TamanoBytes = d.TamanoBytes,
                    TamanoLegible = d.TamanoLegible(),
                    FechaCarga = d.FechaCarga,
                    Certificado = d.Certificado,
                    FechaCertificacion = d.FechaCertificacion,
                    CertificadoPor = d.CertificadoPor != null ? $"{d.CertificadoPor.Apellido}, {d.CertificadoPor.Nombre}" : null
                }).ToList(),
                Historial = solicitud.Historial.OrderByDescending(h => h.FechaCambio).Select(h => new HistorialCambioDto
                {
                    FechaCambio = h.FechaCambio,
                    EstadoAnterior = h.EstadoAnterior.ObtenerDescripcion(),
                    EstadoNuevo = h.EstadoNuevo.ObtenerDescripcion(),
                    Usuario = $"{h.Usuario.Apellido}, {h.Usuario.Nombre}",
                    Comentario = h.Comentario
                }).ToList(),
                PuedeEditar = solicitud.PuedeSerEditada(),
                PuedeEnviar = solicitud.PuedeSerEnviada(),
                PuedeCancelar = solicitud.Estado == EstadoSolicitud.Borrador || solicitud.Estado == EstadoSolicitud.Enviada
            };

            // Agregar detalles específicos según tipo de subsidio
            detalle.DetalleEspecifico = solicitud.TipoSubsidio switch
            {
                TipoSubsidio.Matrimonio => solicitud.SubsidioMatrimonio,
                TipoSubsidio.Maternidad => solicitud.SubsidioMaternidad,
                TipoSubsidio.NacimientoAdopcion => solicitud.SubsidioNacimientoAdopcion,
                TipoSubsidio.HijoDiscapacitado => solicitud.SubsidioHijoDiscapacitado,
                _ => null
            };

            return detalle;
        }

    }

}
