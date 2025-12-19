using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Extensions
{
    // ==========================================
    // EXTENSION METHODS PARA ENUMS
    // ==========================================
    public static class EnumExtensions
    {
        public static string ObtenerDescripcion(this EstadoSolicitud estado)
        {
            return estado switch
            {
                EstadoSolicitud.Borrador => "Borrador",
                EstadoSolicitud.Enviada => "Enviada",
                EstadoSolicitud.EnRevision => "En Revisión",
                EstadoSolicitud.DocumentacionIncompleta => "Documentación Incompleta",
                EstadoSolicitud.Aprobada => "Aprobada",
                EstadoSolicitud.Rechazada => "Rechazada",
                EstadoSolicitud.Pagada => "Pagada",
                _ => "Desconocido"
            };
        }

        public static string ObtenerDescripcion(this TipoSubsidio tipo)
        {
            return tipo switch
            {
                TipoSubsidio.Matrimonio => "Subsidio por Matrimonio",
                TipoSubsidio.Maternidad => "Subsidio por Maternidad",
                TipoSubsidio.NacimientoAdopcion => "Subsidio por Nacimiento/Adopción",
                TipoSubsidio.HijoDiscapacitado => "Subsidio por Hijo Discapacitado",
                _ => "Desconocido"
            };
        }

        public static string ObtenerDescripcion(this TipoDocumento tipo)
        {
            return tipo switch
            {
                TipoDocumento.DNISolicitante => "DNI del Solicitante",
                TipoDocumento.DNIConyuge => "DNI del Cónyuge",
                TipoDocumento.ConstanciaCBU => "Constancia de CBU",
                TipoDocumento.ActaMatrimonio => "Acta de Matrimonio",
                TipoDocumento.ActaNacimiento => "Acta de Nacimiento",
                TipoDocumento.DNIRecienNacido => "DNI del Recién Nacido",
                TipoDocumento.SentenciaAdopcion => "Sentencia de Adopción",
                TipoDocumento.SentenciaFiliacion => "Sentencia de Filiación",
                TipoDocumento.CertificadoDiscapacidad => "Certificado de Discapacidad",
                TipoDocumento.CertificadoSupervivencia => "Certificado de Supervivencia",
                TipoDocumento.AutorizacionTransferencia => "Autorización de Transferencia",
                _ => "Otros"
            };
        }

        public static string ObtenerDescripcion(this RolUsuario rol)
        {
            return rol switch
            {
                RolUsuario.Afiliado => "Afiliado",
                RolUsuario.EmpleadoMesaEntrada => "Empleado de Mesa de Entrada",
                RolUsuario.EmpleadoRevisor => "Empleado Revisor",
                RolUsuario.Administrador => "Administrador",
                _ => "Desconocido"
            };
        }

        public static string ObtenerDescripcion(this EstadoCivil estadoCivil)
        {
            return estadoCivil switch
            {
                EstadoCivil.Soltero => "Soltero/a",
                EstadoCivil.Casado => "Casado/a",
                EstadoCivil.Divorciado => "Divorciado/a",
                EstadoCivil.Viudo => "Viudo/a",
                EstadoCivil.Concubinato => "Concubinato/Unión Convivencial",
                _ => "Desconocido"
            };
        }

        public static string ObtenerClaseBadge(this EstadoSolicitud estado)
        {
            return estado switch
            {
                EstadoSolicitud.Borrador => "badge bg-secondary",
                EstadoSolicitud.Enviada => "badge bg-info",
                EstadoSolicitud.EnRevision => "badge bg-warning text-dark",
                EstadoSolicitud.DocumentacionIncompleta => "badge bg-danger",
                EstadoSolicitud.Aprobada => "badge bg-success",
                EstadoSolicitud.Rechazada => "badge bg-dark",
                EstadoSolicitud.Pagada => "badge bg-primary",
                _ => "badge bg-secondary"
            };
        }

        public static string ObtenerIcono(this TipoSubsidio tipo)
        {
            return tipo switch
            {
                TipoSubsidio.Matrimonio => "bi bi-heart",
                TipoSubsidio.Maternidad => "bi bi-person-heart",
                TipoSubsidio.NacimientoAdopcion => "bi bi-people",
                TipoSubsidio.HijoDiscapacitado => "bi bi-universal-access",
                _ => "bi bi-file-text"
            };
        }

        public static string ObtenerColorEstado(this EstadoSolicitud estado)
        {
            return estado switch
            {
                EstadoSolicitud.Borrador => "#6c757d",
                EstadoSolicitud.Enviada => "#0dcaf0",
                EstadoSolicitud.EnRevision => "#ffc107",
                EstadoSolicitud.DocumentacionIncompleta => "#dc3545",
                EstadoSolicitud.Aprobada => "#198754",
                EstadoSolicitud.Rechazada => "#343a40",
                EstadoSolicitud.Pagada => "#0d6efd",
                _ => "#6c757d"
            };
        }
    }
}
