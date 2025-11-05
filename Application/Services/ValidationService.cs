using Capsap.Domain.Constants;
using Capsap.Domain.Enums;
using Capsap.Domain.Interfaces.Repositories;
using Capsap.Domain.Services;
using Capsap.Domain.Validators;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ValidationService : IValidacionService
    {
        private readonly IAfiliadoRepository _afiliadoRepository;
        private readonly IConfiguracionRepository _configuracionRepository;
        private readonly IValidacionPlazosDomainService _validacionPlazos;
        private readonly IValidacionDocumentacionDomainService _validacionDocumentacion;
        private readonly ISolicitudSubsidioRepository _solicitudRepository;

        public ValidationService(
            IAfiliadoRepository afiliadoRepository,
            IConfiguracionRepository configuracionRepository,
            IValidacionPlazosDomainService validacionPlazos,
            IValidacionDocumentacionDomainService validacionDocumentacion,
            ISolicitudSubsidioRepository solicitudRepository)
        {
            _afiliadoRepository = afiliadoRepository;
            _configuracionRepository = configuracionRepository;
            _validacionPlazos = validacionPlazos;
            _validacionDocumentacion = validacionDocumentacion;
            _solicitudRepository = solicitudRepository;
        }

        public async Task<r> VerificarDeudaAsync(int afiliadoId)
        {
            var afiliado = await _afiliadoRepository.GetByIdAsync(afiliadoId);
            if (afiliado == null)
                return Result.Failure("Afiliado no encontrado");

            if (afiliado.TieneDeuda)
            {
                return Result.Failure(DomainConstants.ErrorMessages.AFILIADO_CON_DEUDA);
            }

            // Actualizar fecha de última verificación
            afiliado.FechaUltimaVerificacionDeuda = DateTime.Now;
            await _afiliadoRepository.UpdateAsync(afiliado);
            await _afiliadoRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<r> ValidarPlazoSolicitudAsync(TipoSubsidio tipoSubsidio, DateTime fechaEvento)
        {
            return _validacionPlazos.ValidarPlazoSolicitud(tipoSubsidio, fechaEvento);
        }

        public async Task<r> ValidarDocumentosRequeridosAsync(int solicitudId)
        {
            var solicitud = await _solicitudRepository.GetByIdWithDetailsAsync(solicitudId);
            if (solicitud == null)
                return Result.Failure("Solicitud no encontrada");

            return _validacionDocumentacion.ValidarDocumentacionCompleta(solicitud);
        }

        public Task<r> ValidarCBUAsync(string cbu)
        {
            if (!DomainValidators.EsCBUValido(cbu))
                return Task.FromResult(Result.Failure(DomainConstants.ErrorMessages.CBU_INVALIDO));

            return Task.FromResult(Result.Success());
        }

        public Task<r> ValidarDNIAsync(string dni)
        {
            if (!DomainValidators.EsDNIValido(dni))
                return Task.FromResult(Result.Failure(DomainConstants.ErrorMessages.DNI_INVALIDO));

            return Task.FromResult(Result.Success());
        }

        public Task<r> ValidarCUILAsync(string cuil)
        {
            if (!DomainValidators.EsCUILValido(cuil))
                return Task.FromResult(Result.Failure(DomainConstants.ErrorMessages.CUIL_INVALIDO));

            return Task.FromResult(Result.Success());
        }
    }
}
