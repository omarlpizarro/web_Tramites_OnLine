using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando ya existe una solicitud activa del mismo tipo
    /// </summary>
    public class SolicitudDuplicadaException : DomainException
    {
        public int AfiliadoId { get; }
        public TipoSubsidio TipoSubsidio { get; }
        public string NumeroSolicitudExistente { get; }

        public SolicitudDuplicadaException(int afiliadoId, TipoSubsidio tipoSubsidio, string numeroSolicitudExistente)
            : base(
                $"Ya existe una solicitud activa de tipo {tipoSubsidio} (Número: {numeroSolicitudExistente}). Debe esperar a que se resuelva antes de crear una nueva.",
                "SOLICITUD_DUPLICADA")
        {
            AfiliadoId = afiliadoId;
            TipoSubsidio = tipoSubsidio;
            NumeroSolicitudExistente = numeroSolicitudExistente;
            AddData("AfiliadoId", afiliadoId);
            AddData("TipoSubsidio", tipoSubsidio);
            AddData("NumeroSolicitudExistente", numeroSolicitudExistente);
        }
    }

}
