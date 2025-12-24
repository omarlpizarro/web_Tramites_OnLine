using Application.DTOs.Response;
using Application.Interfaces;
using Capsap.Domain.Constants;
using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using Capsap.Domain.Extensions;
using Capsap.Domain.Interfaces.Repositories;
using Application.Interfaces.Services;
using Capsap.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DocumentoService : IDocumentoService
    {
        private readonly IDocumentoRepository _documentoRepository;
        private readonly ISolicitudSubsidioRepository _solicitudRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public DocumentoService(
            IDocumentoRepository documentoRepository,
            ISolicitudSubsidioRepository solicitudRepository,
            IUsuarioRepository usuarioRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork)
        {
            _documentoRepository = documentoRepository;
            _solicitudRepository = solicitudRepository;
            _usuarioRepository = usuarioRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<DocumentoDto>> SubirDocumentoAsync(
            int solicitudId,
            TipoDocumento tipoDocumento,
            IFormFile archivo,
            int usuarioId)
        {
            try
            {
                // Validar archivo
                if (archivo == null || archivo.Length == 0)
                    return Result<DocumentoDto>.Failure("El archivo es requerido");

                // Validar tamaño
                if (archivo.Length > DomainConstants.TAMANO_MAXIMO_ARCHIVO_BYTES)
                {
                    var tamanoMB = DomainConstants.TAMANO_MAXIMO_ARCHIVO_BYTES / 1024 / 1024;
                    return Result<DocumentoDto>.Failure($"El archivo excede el tamaño máximo de {tamanoMB} MB");
                }

                // Validar extensión
                var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
                if (!DomainConstants.EXTENSIONES_PERMITIDAS.Contains(extension))
                {
                    return Result<DocumentoDto>.Failure(
                        $"Extensión no permitida. Solo se aceptan: {string.Join(", ", DomainConstants.EXTENSIONES_PERMITIDAS)}"
                    );
                }

                await _unitOfWork.BeginTransactionAsync();

                // Verificar que la solicitud existe
                var solicitud = await _solicitudRepository.ObtenerPorIdAsync(solicitudId);
                if (solicitud == null)
                    return Result<DocumentoDto>.Failure("Solicitud no encontrada");

                // Verificar que el usuario tiene permisos
                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                    return Result<DocumentoDto>.Failure("Usuario no encontrado");

                if (usuario.Rol == RolUsuario.Afiliado && solicitud.AfiliadoSolicitanteId != usuario.AfiliadoId)
                    return Result<DocumentoDto>.Failure("No tiene permisos para subir documentos a esta solicitud");

                // Guardar archivo en storage
                string rutaArchivo;
                using (var stream = archivo.OpenReadStream())
                {
                    rutaArchivo = await _fileStorageService.GuardarDocumentoSolicitudAsync(
                        solicitudId,
                        stream,
                        archivo.FileName);
                }

                // Crear entidad documento
                var documento = new DocumentoAdjunto
                {
                    SolicitudSubsidioId = solicitudId,
                    TipoDocumento = tipoDocumento,
                    NombreArchivo = archivo.FileName,
                    RutaArchivo = rutaArchivo,
                    ContentType = archivo.ContentType,
                    TamanoBytes = archivo.Length,
                    FechaCarga = DateTime.Now,
                    Certificado = false,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                };

                await _documentoRepository.AgregarAsync(documento);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var dto = MapearADocumentoDto(documento);
                return Result<DocumentoDto>.Success(dto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<DocumentoDto>.Failure($"Error al subir documento: {ex.Message}");
            }
        }

        public async Task<Result<ArchivoDto>> ObtenerDocumentoAsync(int documentoId, int usuarioId)
        {
            try
            {
                var documento = await _documentoRepository.ObtenerPorIdAsync(documentoId);
                if (documento == null)
                    return Result<ArchivoDto>.Failure("Documento no encontrado");

                // Verificar permisos
                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                    return Result<ArchivoDto>.Failure("Usuario no encontrado");

                var solicitud = documento.Solicitud;
                if (usuario.Rol == RolUsuario.Afiliado && solicitud.AfiliadoSolicitanteId != usuario.AfiliadoId)
                    return Result<ArchivoDto>.Failure("No tiene permisos para acceder a este documento");

                // Obtener archivo del storage
                var contenido = await _fileStorageService.ObtenerDocumentoSolicitudAsync(
                    solicitud.Id,
                    documento.NombreArchivo);

                var archivoDto = new ArchivoDto
                {
                    Contenido = contenido,
                    NombreArchivo = documento.NombreArchivo,
                    ContentType = documento.ContentType
                };

                return Result<ArchivoDto>.Success(archivoDto);
            }
            catch (Exception ex)
            {
                return Result<ArchivoDto>.Failure($"Error al obtener documento: {ex.Message}");
            }
        }

        public async Task<Result> CertificarDocumentoAsync(int documentoId, int usuarioId)
        {
            // Obtener el documento
            var documento = await _documentoRepository.ObtenerPorIdAsync(documentoId);
            if (documento == null)
                return Result.Failure("Documento no encontrado");

            // Verificar que no esté ya certificado
            if (documento.Certificado)
                return Result.Failure("El documento ya está certificado");

            // Verificar permisos del usuario
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                return Result.Failure("Usuario no encontrado");

            // Solo empleados y administradores pueden certificar
            if (usuario.Rol != RolUsuario.EmpleadoRevisor && usuario.Rol != RolUsuario.Administrador)
                return Result.Failure("No tiene permisos para certificar documentos");

            // Certificar el documento
            documento.Certificado = true;
            documento.CertificadoPorUsuarioId = usuarioId;
            documento.FechaCertificacion = DateTime.UtcNow;

            // Guardar cambios
            await _documentoRepository.ActualizarAsync(documento);

            // Opcional: Agregar al historial de la solicitud
            var historial = new HistorialSolicitud
            {
                SolicitudSubsidioId = documento.SolicitudSubsidioId,
                EstadoNuevo = documento.Solicitud.Estado, // Mantener el estado actual
                Comentario = $"Documento '{documento.TipoDocumento}' certificado por {usuario.Nombre}",
                UsuarioId = usuarioId,
                FechaCambio = DateTime.UtcNow
            };

            await _solicitudRepository.AgregarHistorialAsync(historial);

            return Result.Success();
        }

        public async Task<Result> EliminarDocumentoAsync(int documentoId, int usuarioId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var documento = await _documentoRepository.ObtenerPorIdAsync(documentoId);
                if (documento == null)
                    return Result.Failure("Documento no encontrado");

                // Verificar permisos
                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                    return Result.Failure("Usuario no encontrado");

                var solicitud = documento.Solicitud;
                if (usuario.Rol == RolUsuario.Afiliado && solicitud.AfiliadoSolicitanteId != usuario.AfiliadoId)
                    return Result.Failure("No tiene permisos para eliminar este documento");

                // Solo se puede eliminar si la solicitud está en borrador
                if (solicitud.Estado != EstadoSolicitud.Borrador)
                    return Result.Failure("No se puede eliminar documentos de una solicitud enviada");

                // Marcar como inactivo (soft delete)
                documento.Activo = false;
                documento.FechaModificacion = DateTime.Now;

                await _documentoRepository.ActualizarAsync(documento);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure($"Error al eliminar documento: {ex.Message}");
            }
        }

        public async Task<Result<List<DocumentoDto>>> ObtenerDocumentosSolicitudAsync(int solicitudId)
        {
            try
            {
                var documentos = await _documentoRepository.ObtenerPorSolicitudAsync(solicitudId);
                var dtos = documentos
                    .Where(d => d.Activo)
                    .Select(d => MapearADocumentoDto(d))
                    .ToList();

                return Result<List<DocumentoDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return Result<List<DocumentoDto>>.Failure($"Error al obtener documentos: {ex.Message}");
            }
        }

        private DocumentoDto MapearADocumentoDto(DocumentoAdjunto documento)
        {
            return new DocumentoDto
            {
                Id = documento.Id,
                TipoDocumento = documento.TipoDocumento,
                TipoDocumentoDescripcion = documento.TipoDocumento.ObtenerDescripcion(),
                NombreArchivo = documento.NombreArchivo,
                ContentType = documento.ContentType,
                TamanoBytes = documento.TamanoBytes,
                TamanoLegible = documento.TamanoLegible(),
                FechaCarga = documento.FechaCarga,
                Certificado = documento.Certificado,
                FechaCertificacion = documento.FechaCertificacion,
                CertificadoPor = documento.CertificadoPor != null
                    ? $"{documento.CertificadoPor.Apellido}, {documento.CertificadoPor.Nombre}"
                    : null
            };
        }

    }
}
