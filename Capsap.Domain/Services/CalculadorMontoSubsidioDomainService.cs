using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    public class CalculadorMontoSubsidioDomainService : ICalculadorMontoSubsidioDomainService
    {
        public decimal CalcularMontoSubsidio(SolicitudSubsidio solicitud)
        {
            var montoBase = ObtenerMontoBase(solicitud.TipoSubsidio);
            var montoFinal = AplicarBonificaciones(solicitud, montoBase);
            return montoFinal;
        }

        public decimal ObtenerMontoBase(TipoSubsidio tipoSubsidio)
        {
            // Estos valores deberían venir de configuración o base de datos
            return tipoSubsidio switch
            {
                TipoSubsidio.Matrimonio => 50000m,
                TipoSubsidio.Maternidad => 30000m,
                TipoSubsidio.NacimientoAdopcion => 40000m,
                TipoSubsidio.HijoDiscapacitado => 60000m,
                _ => 0m
            };
        }

        public decimal AplicarBonificaciones(SolicitudSubsidio solicitud, decimal montoBase)
        {
            decimal montoFinal = montoBase;

            // Ejemplo: Si ambos cónyuges son afiliados, bonificación del 20%
            if (solicitud.TipoSubsidio == TipoSubsidio.Matrimonio &&
                solicitud.SubsidioMatrimonio?.AmbosAfiliadosActivos == true)
            {
                montoFinal *= 1.20m;
            }

            // Ejemplo: Múltiples hijos en nacimiento/adopción
            if (solicitud.TipoSubsidio == TipoSubsidio.NacimientoAdopcion)
            {
                var cantidadHijos = solicitud.SubsidioNacimientoAdopcion?.Hijos?.Count ?? 1;
                if (cantidadHijos > 1)
                {
                    // Bonificación del 50% por cada hijo adicional
                    montoFinal += (montoBase * 0.5m * (cantidadHijos - 1));
                }
            }

            return Math.Round(montoFinal, 2);
        }
    }

}
