using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    // ==========================================
    // SERVICIO DE GENERACIÓN DE NÚMEROS DE SOLICITUD
    // ==========================================
    public interface IGeneradorNumeroSolicitudDomainService
    {
        string GenerarNumeroSolicitud(TipoSubsidio tipoSubsidio, int correlativo);
        string ObtenerPrefijo(TipoSubsidio tipoSubsidio);
        string ObtenerPeriodo();
    }

}
