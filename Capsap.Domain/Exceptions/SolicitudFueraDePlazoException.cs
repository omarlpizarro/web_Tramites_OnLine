using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    // ==========================================
    // EXCEPCIONES DE SOLICITUD
    // ==========================================

    /// <summary>
    /// Excepción lanzada cuando una solicitud está fuera del plazo permitido (180 días)
    /// </summary>
    public class SolicitudFueraDePlazoException : DomainException
    {
        public int DiasTranscurridos { get; }
        public int PlazoMaximo { get; }
        public DateTime FechaEvento { get; }
        public TipoSubsidio TipoSubsidio { get; }

        public SolicitudFueraDePlazoException(int diasTranscurridos, int plazoMaximo)
            : base(
                $"La solicitud está fuera de plazo. Han transcurrido {diasTranscurridos} días. El plazo máximo es de {plazoMaximo} días corridos.",
                "SOLICITUD_FUERA_DE_PLAZO")
        {
            DiasTranscurridos = diasTranscurridos;
            PlazoMaximo = plazoMaximo;
            AddData("DiasTranscurridos", diasTranscurridos);
            AddData("PlazoMaximo", plazoMaximo);
        }

        public SolicitudFueraDePlazoException(DateTime fechaEvento, TipoSubsidio tipoSubsidio, int diasTranscurridos, int plazoMaximo)
            : base(
                $"La solicitud de {tipoSubsidio} está fuera de plazo. El evento ocurrió el {fechaEvento:dd/MM/yyyy} ({diasTranscurridos} días). El plazo máximo es de {plazoMaximo} días corridos.",
                "SOLICITUD_FUERA_DE_PLAZO")
        {
            FechaEvento = fechaEvento;
            TipoSubsidio = tipoSubsidio;
            DiasTranscurridos = diasTranscurridos;
            PlazoMaximo = plazoMaximo;
            AddData("FechaEvento", fechaEvento);
            AddData("TipoSubsidio", tipoSubsidio);
            AddData("DiasTranscurridos", diasTranscurridos);
            AddData("PlazoMaximo", plazoMaximo);
        }
    }

}
