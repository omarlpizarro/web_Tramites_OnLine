using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class CrearSolicitudMaternidadDto
    {
        public int AfiliadoSolicitanteId { get; set; }

        // Tipo de evento
        public TipoEventoMaternidad TipoEvento { get; set; }
        public DateTime FechaEvento { get; set; }

        // Datos del hijo
        public string NombreHijo { get; set; }
        public string DNIHijo { get; set; }
        public string CUILHijo { get; set; }

        // Acta de nacimiento
        public string ActaNumero { get; set; }
        public string Tomo { get; set; }
        public string Anio { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }

        // ✅ AGREGADAS: Datos de adopción (si aplica)
        public DateTime? FechaSentenciaJudicial { get; set; }
        public string CiudadSentencia { get; set; }
        public string ProvinciaSentencia { get; set; }

        // Datos bancarios
        public string CBU { get; set; }
        public string TipoCuenta { get; set; }
        public string Banco { get; set; }

        // ✅ AGREGADA: Observaciones
        public string Observaciones { get; set; }
    }

}
