using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class CrearSolicitudDiscapacidadDto
    {
        public int AfiliadoSolicitanteId { get; set; }
        public TipoSolicitudDiscapacidad TipoSolicitud { get; set; }
        public string NombreHijo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string DNI { get; set; }
        public string CUIL { get; set; }
        public string NumeroCertificadoDiscapacidad { get; set; }
        public string Diagnostico { get; set; }
        public DateTime FechaEmisionCertificado { get; set; }
        public DateTime FechaVencimientoCertificado { get; set; }
        public string LugarEmision { get; set; }

        // Datos bancarios
        public string CBU { get; set; }
        public string TipoCuenta { get; set; }
        public string Banco { get; set; }
    }

}
