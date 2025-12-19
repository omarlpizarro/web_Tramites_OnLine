using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    // ==========================================
    // DTOs DE REQUEST
    // ==========================================
    public class CrearSolicitudMatrimonioDto
    {
        public int AfiliadoSolicitanteId { get; set; }
        public int? AfiliadoConyuge2Id { get; set; }

        // Datos del matrimonio
        public DateTime FechaCelebracion { get; set; }
        public string ActaNumero { get; set; }
        public string Tomo { get; set; }
        public string Anio { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }

        // Datos del cónyuge
        public string DNIConyuge { get; set; }
        public string CUILConyuge { get; set; }
        public bool AmbosAfiliadosActivos { get; set; }

        // Datos bancarios
        public string CBU { get; set; }
        public string TipoCuenta { get; set; }
        public string Banco { get; set; }
        public bool TransferenciaATercero { get; set; }
        public string TitularCuenta { get; set; }
        public string CUITTitular { get; set; }

        // ? AGREGADA: Observaciones
        public string Observaciones { get; set; }
    }
}
