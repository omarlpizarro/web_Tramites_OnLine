using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    // ==========================================
    // SUBSIDIO MATRIMONIO - Factory Method
    // ==========================================
    public partial class SubsidioMatrimonio : EntityBase
    {
        private const int PLAZO_MAXIMO_DIAS = 180;

        /// <summary>
        /// Factory Method para crear subsidio de matrimonio con validaciones
        /// </summary>
        public static Result<SubsidioMatrimonio> Crear(
            SolicitudSubsidio solicitud,
            DateTime fechaCelebracion,
            string actaNumero,
            string tomo,
            string anio,
            string ciudad,
            string provincia,
            string dniConyuge,
            string cuilConyuge,
            bool ambosAfiliadosActivos = false)
        {
            // Validación: Fecha de celebración no puede ser futura
            if (fechaCelebracion > DateTime.Now)
            {
                return Result<SubsidioMatrimonio>.Failure("La fecha de celebración no puede ser futura");
            }

            // Validación: Plazo de 180 días corridos
            var diasTranscurridos = (DateTime.Now - fechaCelebracion).TotalDays;
            if (diasTranscurridos > PLAZO_MAXIMO_DIAS)
            {
                return Result<SubsidioMatrimonio>.Failure(
                    $"Ha transcurrido el plazo de {PLAZO_MAXIMO_DIAS} días corridos desde la celebración del matrimonio. " +
                    $"(Días transcurridos: {Math.Floor(diasTranscurridos)})"
                );
            }

            // Validación: DNI del cónyuge
            if (string.IsNullOrWhiteSpace(dniConyuge))
            {
                return Result<SubsidioMatrimonio>.Failure("Debe proporcionar el DNI del cónyuge");
            }

            var subsidio = new SubsidioMatrimonio
            {
                SolicitudSubsidioId = solicitud.Id,
                FechaCelebracion = fechaCelebracion,
                ActaNumero = actaNumero,
                Tomo = tomo,
                Anio = anio,
                Ciudad = ciudad,
                Provincia = provincia,
                DNIConyuge = dniConyuge,
                CUILConyuge = cuilConyuge,
                AmbosAfiliadosActivos = ambosAfiliadosActivos,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            return Result<SubsidioMatrimonio>.Success(subsidio);
        }

        public bool EstaDentroDePlazo()
        {
            var diasTranscurridos = (DateTime.Now - FechaCelebracion).TotalDays;
            return diasTranscurridos <= PLAZO_MAXIMO_DIAS;
        }
    }
}
