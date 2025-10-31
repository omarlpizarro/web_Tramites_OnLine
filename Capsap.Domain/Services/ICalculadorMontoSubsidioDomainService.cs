using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    // ==========================================
    // SERVICIO DE CÁLCULO DE MONTOS (ejemplo para futura implementación)
    // ==========================================
    public interface ICalculadorMontoSubsidioDomainService
    {
        decimal CalcularMontoSubsidio(SolicitudSubsidio solicitud);
        decimal ObtenerMontoBase(TipoSubsidio tipoSubsidio);
        decimal AplicarBonificaciones(SolicitudSubsidio solicitud, decimal montoBase);
    }

}
