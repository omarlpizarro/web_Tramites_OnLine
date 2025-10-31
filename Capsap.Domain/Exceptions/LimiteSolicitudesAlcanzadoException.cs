using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{

    // ==========================================
    // EXCEPCIONES DE NEGOCIO
    // ==========================================

    /// <summary>
    /// Excepción lanzada cuando se alcanza el límite de solicitudes
    /// </summary>
    public class LimiteSolicitudesAlcanzadoException : DomainException
    {
        public int AfiliadoId { get; }
        public TipoSubsidio TipoSubsidio { get; }
        public int LimiteAnual { get; }

        public LimiteSolicitudesAlcanzadoException(int afiliadoId, TipoSubsidio tipoSubsidio, int limiteAnual)
            : base(
                $"Ha alcanzado el límite de {limiteAnual} solicitud(es) de tipo {tipoSubsidio} por año.",
                "LIMITE_SOLICITUDES_ALCANZADO")
        {
            AfiliadoId = afiliadoId;
            TipoSubsidio = tipoSubsidio;
            LimiteAnual = limiteAnual;
            AddData("AfiliadoId", afiliadoId);
            AddData("TipoSubsidio", tipoSubsidio);
            AddData("LimiteAnual", limiteAnual);
        }
    }

}
