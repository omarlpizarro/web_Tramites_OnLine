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
    // SERVICIO DE VALIDACIÓN DE DOCUMENTACIÓN
    // ==========================================
    public interface IValidacionDocumentacionDomainService
    {
        Result ValidarDocumentacionCompleta(SolicitudSubsidio solicitud);
        List<TipoDocumento> ObtenerDocumentosRequeridos(TipoSubsidio tipoSubsidio);
        List<TipoDocumento> ObtenerDocumentosFaltantes(SolicitudSubsidio solicitud);
        bool TieneDocumentoRequerido(SolicitudSubsidio solicitud, TipoDocumento tipoDocumento);
    }

}
