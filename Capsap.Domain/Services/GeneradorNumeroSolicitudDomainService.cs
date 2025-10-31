using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    public class GeneradorNumeroSolicitudDomainService : IGeneradorNumeroSolicitudDomainService
    {
        public string GenerarNumeroSolicitud(TipoSubsidio tipoSubsidio, int correlativo)
        {
            var prefijo = ObtenerPrefijo(tipoSubsidio);
            var periodo = ObtenerPeriodo();
            var numeroFormateado = correlativo.ToString("D4"); // 4 dígitos con ceros a la izquierda

            return $"{prefijo}-{periodo}-{numeroFormateado}";
        }

        public string ObtenerPrefijo(TipoSubsidio tipoSubsidio)
        {
            return tipoSubsidio switch
            {
                TipoSubsidio.Matrimonio => "MAT",
                TipoSubsidio.Maternidad => "MAN",
                TipoSubsidio.NacimientoAdopcion => "NAC",
                TipoSubsidio.HijoDiscapacitado => "DIS",
                _ => "OTR"
            };
        }

        public string ObtenerPeriodo()
        {
            var ahora = DateTime.Now;
            return $"{ahora.Year}{ahora.Month:D2}"; // YYYYMM
        }
    }
}
