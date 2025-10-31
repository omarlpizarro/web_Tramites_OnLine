using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se cumple el intervalo mínimo entre solicitudes
    /// </summary>
    public class IntervaloMinimoNoAlcanzadoException : DomainException
    {
        public int AfiliadoId { get; }
        public TipoSubsidio TipoSubsidio { get; }
        public int DiasFaltantes { get; }

        public IntervaloMinimoNoAlcanzadoException(int afiliadoId, TipoSubsidio tipoSubsidio, int diasFaltantes)
            : base(
                $"Debe esperar {diasFaltantes} días más para solicitar nuevamente el subsidio de tipo {tipoSubsidio}.",
                "INTERVALO_MINIMO_NO_ALCANZADO")
        {
            AfiliadoId = afiliadoId;
            TipoSubsidio = tipoSubsidio;
            DiasFaltantes = diasFaltantes;
            AddData("AfiliadoId", afiliadoId);
            AddData("TipoSubsidio", tipoSubsidio);
            AddData("DiasFaltantes", diasFaltantes);
        }
    }

}
