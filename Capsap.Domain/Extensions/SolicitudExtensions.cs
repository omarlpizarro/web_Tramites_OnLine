using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Extensions
{
    // ==========================================
    // EXTENSION METHODS PARA SOLICITUDES
    // ==========================================
   
    public static class SolicitudExtensions
    {
        public static bool PuedeSerEditada(this SolicitudSubsidio solicitud)
        {
            return solicitud.Estado == EstadoSolicitud.Borrador;
        }

        public static bool PuedeSerEnviada(this SolicitudSubsidio solicitud)
        {
            return solicitud.Estado == EstadoSolicitud.Borrador &&
                   solicitud.Documentos.Any(d => d.Activo);
        }

        public static bool PuedeSerAprobada(this SolicitudSubsidio solicitud)
        {
            return solicitud.Estado == EstadoSolicitud.EnRevision;
        }

        public static bool PuedeSerRechazada(this SolicitudSubsidio solicitud)
        {
            return solicitud.Estado != EstadoSolicitud.Aprobada &&
                   solicitud.Estado != EstadoSolicitud.Pagada &&
                   solicitud.Estado != EstadoSolicitud.Rechazada;
        }

        public static bool EstaPendienteDeRevision(this SolicitudSubsidio solicitud)
        {
            return solicitud.Estado == EstadoSolicitud.Enviada ||
                   solicitud.Estado == EstadoSolicitud.EnRevision ||
                   solicitud.Estado == EstadoSolicitud.DocumentacionIncompleta;
        }

        public static bool EstaFinalizada(this SolicitudSubsidio solicitud)
        {
            return solicitud.Estado == EstadoSolicitud.Aprobada ||
                   solicitud.Estado == EstadoSolicitud.Rechazada ||
                   solicitud.Estado == EstadoSolicitud.Pagada;
        }

        public static int DiasEnTramite(this SolicitudSubsidio solicitud)
        {
            var fechaFin = solicitud.FechaResolucion ?? DateTime.Now;
            return (fechaFin - solicitud.FechaSolicitud).Days;
        }

        public static bool TieneTodosLosDocumentosRequeridos(this SolicitudSubsidio solicitud)
        {
            var documentosActivos = solicitud.Documentos.Where(d => d.Activo).ToList();

            // Verificar documentos comunes
            if (!documentosActivos.Any(d => d.TipoDocumento == TipoDocumento.DNISolicitante))
                return false;

            if (!documentosActivos.Any(d => d.TipoDocumento == TipoDocumento.ConstanciaCBU))
                return false;

            // Verificar documentos específicos según tipo
            return solicitud.TipoSubsidio switch
            {
                TipoSubsidio.Matrimonio => documentosActivos.Any(d => d.TipoDocumento == TipoDocumento.ActaMatrimonio),
                TipoSubsidio.Maternidad => documentosActivos.Any(d => d.TipoDocumento == TipoDocumento.ActaNacimiento),
                TipoSubsidio.NacimientoAdopcion => documentosActivos.Any(d => d.TipoDocumento == TipoDocumento.ActaNacimiento),
                TipoSubsidio.HijoDiscapacitado => documentosActivos.Any(d => d.TipoDocumento == TipoDocumento.CertificadoDiscapacidad),
                _ => true
            };
        }

        public static decimal PorcentajeCompletitud(this SolicitudSubsidio solicitud)
        {
            int totalPasos = 4; // Datos básicos, datos específicos, documentos, envío
            int pasosCompletados = 0;

            // Paso 1: Datos básicos completos
            if (!string.IsNullOrEmpty(solicitud.CBU))
                pasosCompletados++;

            // Paso 2: Datos específicos según tipo
            bool tieneDetalleEspecifico = solicitud.TipoSubsidio switch
            {
                TipoSubsidio.Matrimonio => solicitud.SubsidioMatrimonio != null,
                TipoSubsidio.Maternidad => solicitud.SubsidioMaternidad != null,
                TipoSubsidio.NacimientoAdopcion => solicitud.SubsidioNacimientoAdopcion != null,
                TipoSubsidio.HijoDiscapacitado => solicitud.SubsidioHijoDiscapacitado != null,
                _ => false
            };
            if (tieneDetalleEspecifico)
                pasosCompletados++;

            // Paso 3: Documentos adjuntos
            if (solicitud.Documentos.Any(d => d.Activo))
                pasosCompletados++;

            // Paso 4: Solicitud enviada
            if (solicitud.Estado != EstadoSolicitud.Borrador)
                pasosCompletados++;

            return (decimal)pasosCompletados / totalPasos * 100;
        }
    }
}
