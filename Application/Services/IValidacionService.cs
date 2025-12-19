using Capsap.Domain.Enums;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    // ==========================================
    // SERVICIO DE VALIDACIÓN
    // ==========================================
    public interface IValidacionService
    {
        Task<Result> VerificarDeudaAsync(int afiliadoId);
        Task<Result> ValidarPlazoSolicitudAsync(TipoSubsidio tipoSubsidio, DateTime fechaEvento);
        Task<Result> ValidarDocumentosRequeridosAsync(int solicitudId);
    }

}
