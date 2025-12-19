using Capsap.Domain.Enums;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    // ==========================================
    // SERVICIO DE VALIDACIÓN DE PLAZOS
    // ==========================================
    public interface IValidacionPlazosDomainService
    {
        Result ValidarPlazoSolicitud(TipoSubsidio tipoSubsidio, DateTime fechaEvento);
        int ObtenerDiasRestantes(TipoSubsidio tipoSubsidio, DateTime fechaEvento);
        bool EstaDentroDePlazo(TipoSubsidio tipoSubsidio, DateTime fechaEvento);
    }

}
