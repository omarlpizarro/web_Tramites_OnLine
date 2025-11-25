using Capsap.Domain.Enums;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    // ==========================================
    // SOLICITUD SUBSIDIO - Factory Method
    // ==========================================
    public partial class SolicitudSubsidio : EntityBase
    {
        // Constructor privado para forzar uso de factory method
        private SolicitudSubsidio()
        {
            Documentos = new List<DocumentoAdjunto>();
            Historial = new List<HistorialSolicitud>();
        }

        /// <summary>
        /// Factory Method para crear una nueva solicitud de subsidio
        /// </summary>
        public static Result<SolicitudSubsidio> Crear(
            Afiliado afiliadoSolicitante,
            TipoSubsidio tipoSubsidio,
            string cbu,
            string tipoCuenta,
            string banco)
        {
            // Validación: Afiliado no debe tener deuda (Art. 73 Ley 4764/94)
            if (afiliadoSolicitante.TieneDeuda)
            {
                return Result<SolicitudSubsidio>.Failure(
                    "El afiliado tiene deuda pendiente. Debe regularizar su situación antes de solicitar beneficios. (Art. 73 Ley 4764/94)"
                );
            }

            // Validación: Afiliado debe estar activo
            if (!afiliadoSolicitante.Activo)
            {
                return Result<SolicitudSubsidio>.Failure("El afiliado no está activo en el sistema");
            }

            // Validación: CBU válido
            if (!CBU.EsValido(cbu))
            {
                return Result<SolicitudSubsidio>.Failure("El CBU proporcionado no es válido");
            }

            var solicitud = new SolicitudSubsidio
            {
                AfiliadoSolicitanteId = afiliadoSolicitante.Id,
                AfiliadoSolicitante = afiliadoSolicitante,
                TipoSubsidio = tipoSubsidio,
                Estado = EstadoSolicitud.Borrador,
                FechaSolicitud = DateTime.Now,
                CBU = cbu,
                TipoCuenta = tipoCuenta,
                Banco = banco,
                FechaCreacion = DateTime.Now,
                Activo = true,
                Documentos = new List<DocumentoAdjunto>(),
                Historial = new List<HistorialSolicitud>()
            };

            return Result<SolicitudSubsidio>.Success(solicitud);
        }

        /// <summary>
        /// Método para enviar la solicitud (cambia de Borrador a Enviada)
        /// </summary>
        public Result Enviar()
        {
            if (Estado != EstadoSolicitud.Borrador)
            {
                return Result.Failure("Solo se pueden enviar solicitudes en estado borrador");
            }

            // Validar que tenga documentos adjuntos
            if (!Documentos.Any() || Documentos.All(d => !d.Activo))
            {
                return Result.Failure("Debe adjuntar al menos un documento antes de enviar la solicitud");
            }

            Estado = EstadoSolicitud.Enviada;
            FechaModificacion = DateTime.Now;

            return Result.Success();
        }

        /// <summary>
        /// Método para aprobar la solicitud
        /// </summary>
        public Result Aprobar(Usuario usuario, string comentario = null)
        {
            if (Estado != EstadoSolicitud.EnRevision)
            {
                return Result.Failure("Solo se pueden aprobar solicitudes en estado de revisión");
            }

            if (usuario.Rol != RolUsuario.EmpleadoRevisor && usuario.Rol != RolUsuario.Administrador)
            {
                return Result.Failure("No tiene permisos para aprobar solicitudes");
            }

            Estado = EstadoSolicitud.Aprobada;
            FechaResolucion = DateTime.Now;
            ObservacionesInternas = comentario;
            FechaModificacion = DateTime.Now;

            return Result.Success();
        }

        /// <summary>
        /// Método para rechazar la solicitud
        /// </summary>
        public Result Rechazar(Usuario usuario, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
            {
                return Result.Failure("Debe proporcionar un motivo para el rechazo");
            }

            if (Estado == EstadoSolicitud.Aprobada || Estado == EstadoSolicitud.Pagada)
            {
                return Result.Failure("No se puede rechazar una solicitud aprobada o pagada");
            }

            Estado = EstadoSolicitud.Rechazada;
            FechaResolucion = DateTime.Now;
            Observaciones = motivo;
            FechaModificacion = DateTime.Now;

            return Result.Success();
        }
    }

}
