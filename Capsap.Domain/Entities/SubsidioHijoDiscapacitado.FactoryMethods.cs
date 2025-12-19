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
    // SUBSIDIO HIJO DISCAPACITADO - Factory Method (YA EXISTÍA)
    // ==========================================
    public partial class SubsidioHijoDiscapacitado : EntityBase
    {
        /// <summary>
        /// Factory Method ya implementado anteriormente
        /// Este está correcto y no necesita cambios
        /// </summary>
        public static Result<SubsidioHijoDiscapacitado> Crear(
            SolicitudSubsidio solicitud,
            TipoSolicitudDiscapacidad tipoSolicitud,
            string nombre,
            DateTime fechaNacimiento,
            string dni,
            string numeroCertificadoDiscapacidad,
            string diagnostico,
            DateTime fechaEmisionCertificado,
            DateTime fechaVencimientoCertificado,
            string lugarEmision)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return Result<SubsidioHijoDiscapacitado>.Failure("Debe proporcionar el nombre del hijo");
            }

            if (string.IsNullOrWhiteSpace(numeroCertificadoDiscapacidad))
            {
                return Result<SubsidioHijoDiscapacitado>.Failure("Debe proporcionar el número de certificado de discapacidad");
            }

            // Validación: Certificado no debe estar vencido
            if (fechaVencimientoCertificado < DateTime.Now)
            {
                return Result<SubsidioHijoDiscapacitado>.Failure(
                    $"El certificado de discapacidad está vencido. Fecha de vencimiento: {fechaVencimientoCertificado:dd/MM/yyyy}"
                );
            }

            // Validación: Fecha de emisión debe ser anterior a la fecha de vencimiento
            if (fechaEmisionCertificado >= fechaVencimientoCertificado)
            {
                return Result<SubsidioHijoDiscapacitado>.Failure(
                    "La fecha de emisión debe ser anterior a la fecha de vencimiento"
                );
            }

            var subsidio = new SubsidioHijoDiscapacitado
            {
                SolicitudSubsidioId = solicitud.Id,
                TipoSolicitud = tipoSolicitud,
                Nombre = nombre,
                FechaNacimiento = fechaNacimiento,
                DNI = dni,
                NumeroCertificadoDiscapacidad = numeroCertificadoDiscapacidad,
                Diagnostico = diagnostico,
                FechaEmisionCertificado = fechaEmisionCertificado,
                FechaVencimientoCertificado = fechaVencimientoCertificado,
                LugarEmision = lugarEmision,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            return Result<SubsidioHijoDiscapacitado>.Success(subsidio);
        }

        public bool CertificadoVigente()
        {
            return FechaVencimientoCertificado >= DateTime.Now;
        }

        public int DiasHastaVencimiento()
        {
            return (FechaVencimientoCertificado - DateTime.Now).Days;
        }
    }
}
