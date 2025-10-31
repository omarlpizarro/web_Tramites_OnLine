using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using Capsap.Domain.Extensions;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    public class ValidacionDocumentacionDomainService : IValidacionDocumentacionDomainService
    {
        public Result ValidarDocumentacionCompleta(SolicitudSubsidio solicitud)
        {
            var documentosFaltantes = ObtenerDocumentosFaltantes(solicitud);

            if (documentosFaltantes.Any())
            {
                var nombresFaltantes = documentosFaltantes.Select(d => d.ObtenerDescripcion());
                return Result.Failure(
                    $"Documentación incompleta. Faltan los siguientes documentos: {string.Join(", ", nombresFaltantes)}"
                );
            }

            return Result.Success();
        }

        public List<TipoDocumento> ObtenerDocumentosRequeridos(TipoSubsidio tipoSubsidio)
        {
            var documentosRequeridos = new List<TipoDocumento>
            {
                TipoDocumento.DNISolicitante,
                TipoDocumento.ConstanciaCBU
            };

            switch (tipoSubsidio)
            {
                case TipoSubsidio.Matrimonio:
                    documentosRequeridos.Add(TipoDocumento.ActaMatrimonio);
                    break;

                case TipoSubsidio.Maternidad:
                case TipoSubsidio.NacimientoAdopcion:
                    documentosRequeridos.Add(TipoDocumento.ActaNacimiento);
                    break;

                case TipoSubsidio.HijoDiscapacitado:
                    documentosRequeridos.Add(TipoDocumento.ActaNacimiento);
                    documentosRequeridos.Add(TipoDocumento.CertificadoDiscapacidad);
                    break;
            }

            return documentosRequeridos;
        }

        public List<TipoDocumento> ObtenerDocumentosFaltantes(SolicitudSubsidio solicitud)
        {
            var documentosRequeridos = ObtenerDocumentosRequeridos(solicitud.TipoSubsidio);
            var documentosPresentes = solicitud.Documentos
                .Where(d => d.Activo)
                .Select(d => d.TipoDocumento)
                .Distinct()
                .ToList();

            return documentosRequeridos.Except(documentosPresentes).ToList();
        }

        public bool TieneDocumentoRequerido(SolicitudSubsidio solicitud, TipoDocumento tipoDocumento)
        {
            return solicitud.Documentos.Any(d => d.TipoDocumento == tipoDocumento && d.Activo);
        }
    }

}
