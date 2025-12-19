using Capsap.Domain.Constants;
using Capsap.Domain.Enums;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    public class ValidacionPlazosDomainService : IValidacionPlazosDomainService
    {
        public Result ValidarPlazoSolicitud(TipoSubsidio tipoSubsidio, DateTime fechaEvento)
        {
            var plazoMaximo = ObtenerPlazoMaximo(tipoSubsidio);
            var diasTranscurridos = (DateTime.Now - fechaEvento).TotalDays;

            if (diasTranscurridos > plazoMaximo)
            {
                return Result.Failure(
                    string.Format(DomainConstants.ErrorMessages.SOLICITUD_FUERA_DE_PLAZO, plazoMaximo)
                );
            }

            return Result.Success();
        }

        public int ObtenerDiasRestantes(TipoSubsidio tipoSubsidio, DateTime fechaEvento)
        {
            var plazoMaximo = ObtenerPlazoMaximo(tipoSubsidio);
            var diasTranscurridos = (int)(DateTime.Now - fechaEvento).TotalDays;
            return Math.Max(0, plazoMaximo - diasTranscurridos);
        }

        public bool EstaDentroDePlazo(TipoSubsidio tipoSubsidio, DateTime fechaEvento)
        {
            var plazoMaximo = ObtenerPlazoMaximo(tipoSubsidio);
            var diasTranscurridos = (DateTime.Now - fechaEvento).TotalDays;
            return diasTranscurridos <= plazoMaximo;
        }

        private int ObtenerPlazoMaximo(TipoSubsidio tipoSubsidio)
        {
            return tipoSubsidio switch
            {
                TipoSubsidio.Matrimonio => DomainConstants.PLAZO_MAXIMO_DIAS_MATRIMONIO,
                TipoSubsidio.Maternidad => DomainConstants.PLAZO_MAXIMO_DIAS_MATERNIDAD,
                TipoSubsidio.NacimientoAdopcion => DomainConstants.PLAZO_MAXIMO_DIAS_NACIMIENTO,
                TipoSubsidio.HijoDiscapacitado => 365, // No tiene plazo estricto pero se usa para validación
                _ => 180 // Default
            };
        }
    }
}
