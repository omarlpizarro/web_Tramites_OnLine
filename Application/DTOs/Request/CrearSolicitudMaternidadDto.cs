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
        public TipoEventoMaternidad TipoEvento { get; set; }
        public DateTime FechaEvento { get; set; }
        public string NombreHijo { get; set; }
        public string DNIHijo { get; set; }
        public string CUILHijo { get; set; }
        public string ActaNumero { get; set; }
        public string Tomo { get; set; }
        public string Anio { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }
        public DateTime? FechaSentenciaJudicial { get; set; }

        // Datos bancarios
        public string CBU { get; set; }
        public string TipoCuenta { get; set; }
        public string Banco { get; set; }
    }

}
