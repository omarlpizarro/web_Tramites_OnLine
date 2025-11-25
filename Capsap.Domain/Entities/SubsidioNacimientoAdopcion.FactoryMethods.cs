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
    // SUBSIDIO NACIMIENTO/ADOPCIÓN - Factory Method
    // ==========================================
    public partial class SubsidioNacimientoAdopcion : EntityBase
    {
        private const int PLAZO_MAXIMO_DIAS = 180;

        /// <summary>
        /// Factory Method para crear subsidio de nacimiento/adopción con validaciones
        /// </summary>
        public static Result<SubsidioNacimientoAdopcion> Crear(
            SolicitudSubsidio solicitud,
            TipoEventoNacimiento tipoEvento)
        {
            var subsidio = new SubsidioNacimientoAdopcion
            {
                SolicitudSubsidioId = solicitud.Id,
                TipoEvento = tipoEvento,
                FechaCreacion = DateTime.Now,
                Activo = true,
                Hijos = new List<HijoNacimientoAdopcion>()
            };

            return Result<SubsidioNacimientoAdopcion>.Success(subsidio);
        }

        /// <summary>
        /// Agrega un hijo a la solicitud con validaciones
        /// </summary>
        public Result AgregarHijo(HijoNacimientoAdopcion hijo)
        {
            // Validación: Fecha de nacimiento no puede ser futura
            if (hijo.FechaNacimiento > DateTime.Now)
            {
                return Result.Failure("La fecha de nacimiento no puede ser futura");
            }

            // Validación: Plazo de 180 días
            var fechaEvento = hijo.FechaSentenciaJudicial ?? hijo.FechaNacimiento;
            var diasTranscurridos = (DateTime.Now - fechaEvento).TotalDays;

            if (diasTranscurridos > PLAZO_MAXIMO_DIAS)
            {
                return Result.Failure(
                    $"Ha transcurrido el plazo de {PLAZO_MAXIMO_DIAS} días corridos desde el nacimiento/adopción. " +
                    $"(Días transcurridos: {Math.Floor(diasTranscurridos)})"
                );
            }

            Hijos.Add(hijo);
            return Result.Success();
        }
    }
}
