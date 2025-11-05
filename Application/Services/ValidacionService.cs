using Capsap.Domain.Enums;
using Capsap.Domain.Interfaces.Repositories;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ValidacionService : IValidacionService
    {
        private readonly IAfiliadoRepository _afiliadoRepository;
        private readonly ISolicitudSubsidioRepository _solicitudRepository;
        private readonly IConfiguracionRepository _configuracionRepository;

        public ValidacionService(
            IAfiliadoRepository afiliadoRepository,
            ISolicitudSubsidioRepository solicitudRepository,
            IConfiguracionRepository configuracionRepository)
        {
            _afiliadoRepository = afiliadoRepository;
            _solicitudRepository = solicitudRepository;
            _configuracionRepository = configuracionRepository;
        }

        public async Task<Result> VerificarDeudaAsync(int afiliadoId)
        {
            var afiliado = await _afiliadoRepository.GetByIdAsync(afiliadoId);
            if (afiliado == null)
                return Result.Failure("Afiliado no encontrado");

            // Aquí se integraría con el sistema de deudas existente
            // Por ahora, verificamos el campo TieneDeuda
            if (afiliado.TieneDeuda)
            {
                return Result.Failure(
                    "El afiliado tiene deuda pendiente con la institución. " +
                    "Debe regularizar su situación antes de solicitar beneficios. " +
                    "(Art. 73 Ley 4764/94)"
                );
            }

            // Actualizar fecha de última verificación
            afiliado.FechaUltimaVerificacionDeuda = DateTime.Now;
            await _afiliadoRepository.UpdateAsync(afiliado);
            await _afiliadoRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> ValidarPlazoSolicitudAsync(TipoSubsidio tipoSubsidio, DateTime fechaEvento)
        {
            int plazoMaximo;

            // Obtener plazo desde configuración
            switch (tipoSubsidio)
            {
                case TipoSubsidio.Matrimonio:
                    plazoMaximo = await _configuracionRepository.ObtenerValorEnteroAsync("Subsidio.Matrimonio.PlazoMaximoDias", 180);
                    break;
                case TipoSubsidio.Maternidad:
                    plazoMaximo = await _configuracionRepository.ObtenerValorEnteroAsync("Subsidio.Maternidad.PlazoMaximoDias", 180);
                    break;
                case TipoSubsidio.NacimientoAdopcion:
                    plazoMaximo = await _configuracionRepository.ObtenerValorEnteroAsync("Subsidio.Nacimiento.PlazoMaximoDias", 180);
                    break;
                default:
                    plazoMaximo = 365;
                    break;
            }

            var diasTranscurridos = (DateTime.Now - fechaEvento).TotalDays;

            if (diasTranscurridos > plazoMaximo)
            {
                return Result.Failure(
                    $"La solicitud está fuera de plazo. " +
                    $"Han transcurrido {Math.Floor(diasTranscurridos)} días desde el evento. " +
                    $"El plazo máximo es de {plazoMaximo} días corridos."
                );
            }

            return Result.Success();
        }

        public async Task<Result> ValidarDocumentosRequeridosAsync(int solicitudId)
        {
            var solicitud = await _solicitudRepository.GetByIdWithDetailsAsync(solicitudId);
            if (solicitud == null)
                return Result.Failure("Solicitud no encontrada");

            var documentosFaltantes = new List<string>();

            // Validar según tipo de subsidio
            switch (solicitud.TipoSubsidio)
            {
                case TipoSubsidio.Matrimonio:
                    if (!solicitud.Documentos.Any(d => d.TipoDocumento == TipoDocumento.DNISolicitante && d.Activo))
                        documentosFaltantes.Add("DNI del Solicitante");
                    if (!solicitud.Documentos.Any(d => d.TipoDocumento == TipoDocumento.ActaMatrimonio && d.Activo))
                        documentosFaltantes.Add("Acta de Matrimonio");
                    break;

                case TipoSubsidio.Maternidad:
                case TipoSubsidio.NacimientoAdopcion:
                    if (!solicitud.Documentos.Any(d => d.TipoDocumento == TipoDocumento.DNISolicitante && d.Activo))
                        documentosFaltantes.Add("DNI del Solicitante");
                    if (!solicitud.Documentos.Any(d => d.TipoDocumento == TipoDocumento.ActaNacimiento && d.Activo))
                        documentosFaltantes.Add("Acta de Nacimiento");
                    break;

                case TipoSubsidio.HijoDiscapacitado:
                    if (!solicitud.Documentos.Any(d => d.TipoDocumento == TipoDocumento.DNISolicitante && d.Activo))
                        documentosFaltantes.Add("DNI del Solicitante");
                    if (!solicitud.Documentos.Any(d => d.TipoDocumento == TipoDocumento.CertificadoDiscapacidad && d.Activo))
                        documentosFaltantes.Add("Certificado de Discapacidad");
                    break;
            }

            // CBU es obligatorio para todos
            if (!solicitud.Documentos.Any(d => d.TipoDocumento == TipoDocumento.ConstanciaCBU && d.Activo))
                documentosFaltantes.Add("Constancia de CBU");

            if (documentosFaltantes.Any())
            {
                return Result.Failure(
                    $"Documentación incompleta. Faltan: {string.Join(", ", documentosFaltantes)}"
                );
            }

            return Result.Success();
        }
    }
}
