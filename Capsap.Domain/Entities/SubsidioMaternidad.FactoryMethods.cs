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
    // SUBSIDIO MATERNIDAD - Factory Method
    // ==========================================
    public partial class SubsidioMaternidad : EntityBase
    {
        private const int PLAZO_MAXIMO_DIAS = 180;

        /// <summary>
        /// Factory Method para crear subsidio de maternidad con validaciones
        /// </summary>
        public static Result<SubsidioMaternidad> Crear(
            SolicitudSubsidio solicitud,
            TipoEventoMaternidad tipoEvento,
            DateTime fechaEvento,
            string nombreHijo,
            string actaNumero,
            string ciudad,
            string provincia)
        {
            // Validación: Fecha del evento no puede ser futura
            if (fechaEvento > DateTime.Now)
            {
                return Result<SubsidioMaternidad>.Failure("La fecha del evento no puede ser futura");
            }

            // Validación: Plazo de 180 días corridos
            var diasTranscurridos = (DateTime.Now - fechaEvento).TotalDays;
            if (diasTranscurridos > PLAZO_MAXIMO_DIAS)
            {
                return Result<SubsidioMaternidad>.Failure(
                    $"Ha transcurrido el plazo de {PLAZO_MAXIMO_DIAS} días corridos desde el {tipoEvento}. " +
                    $"(Días transcurridos: {Math.Floor(diasTranscurridos)})"
                );
            }

            // Validación: Nombre del hijo
            if (string.IsNullOrWhiteSpace(nombreHijo))
            {
                return Result<SubsidioMaternidad>.Failure("Debe proporcionar el nombre del hijo/a");
            }

            var subsidio = new SubsidioMaternidad
            {
                SolicitudSubsidioId = solicitud.Id,
                TipoEvento = tipoEvento,
                FechaEvento = fechaEvento,
                NombreHijo = nombreHijo,
                ActaNumero = actaNumero,
                Ciudad = ciudad,
                Provincia = provincia,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            return Result<SubsidioMaternidad>.Success(subsidio);
        }

        public bool EstaDentroDePlazo()
        {
            var diasTranscurridos = (DateTime.Now - FechaEvento).TotalDays;
            return diasTranscurridos <= PLAZO_MAXIMO_DIAS;
        }
    }
}
