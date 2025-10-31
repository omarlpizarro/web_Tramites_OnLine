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
    // SERVICIO DE REGLAS DE NEGOCIO DE AFILIADOS
    // ==========================================
    public interface IAfiliadoDomainService
    {
        Result PuedeRealizarSolicitud(Afiliado afiliado);
        Result VerificarEstadoAfiliacion(Afiliado afiliado);
        bool TieneSolicitudesActivasDelMismoTipo(Afiliado afiliado, TipoSubsidio tipo, IEnumerable<SolicitudSubsidio> solicitudes);
    }

}
