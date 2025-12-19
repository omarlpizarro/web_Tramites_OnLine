using Capsap.Domain.Entities;
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
    // SERVICIO DE POLÍTICAS DE NEGOCIO
    // ==========================================
    public interface IPoliticasNegocioDomainService
    {
        Result PuedeCrearMultiplesSolicitudes(Afiliado afiliado, TipoSubsidio tipo, IEnumerable<SolicitudSubsidio> solicitudesExistentes);
        Result ValidarIntervaloEntreSolicitudes(Afiliado afiliado, TipoSubsidio tipo, IEnumerable<SolicitudSubsidio> solicitudesExistentes);
        int ObtenerLimiteSolicitudesAnuales(TipoSubsidio tipo);
    }

}
