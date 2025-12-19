using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class CrearSolicitudNacimientoDto
    {
        public int AfiliadoSolicitanteId { get; set; }
        public int? AfiliadoConyuge2Id { get; set; }
        public TipoEventoNacimiento TipoEvento { get; set; }
        public List<DatosHijoDto> Hijos { get; set; } = new List<DatosHijoDto>();

        // Datos bancarios
        public string CBU { get; set; }
        public string TipoCuenta { get; set; }
        public string Banco { get; set; }

        // ? AGREGADA: Observaciones
        public string Observaciones { get; set; }
    }

}
