using Capsap.Domain.Enums;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // ==========================================
    // SERVICIO DE VALIDACIÓN
    // ==========================================
    public interface IValidacionService
    {
        Task<Result> VerificarDeudaAsync(int afiliadoId);
        Task<Result> ValidarPlazoSolicitudAsync(TipoSubsidio tipoSubsidio, System.DateTime fechaEvento);
        Task<Result> ValidarDocumentosRequeridosAsync(int solicitudId);
        Task<Result> ValidarCBUAsync(string cbu);
        Task<Result> ValidarDNIAsync(string dni);
        Task<Result> ValidarCUILAsync(string cuil);
    }
}
