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

        // Datos del hijo
        public string NombreHijo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string DNI { get; set; }
        public string CUIL { get; set; }

        // ? AGREGADAS: Acta de nacimiento
        public string ActaNumero { get; set; }
        public string Tomo { get; set; }
        public string Anio { get; set; }
        public string CiudadNacimiento { get; set; }
        public string ProvinciaNacimiento { get; set; }

        // Adopción (si aplica)
        public DateTime? FechaSentenciaJudicial { get; set; }
        public string CiudadSentencia { get; set; }
        public string ProvinciaSentencia { get; set; }

        // Certificado de Discapacidad
        public string NumeroCertificadoDiscapacidad { get; set; }
        public string Diagnostico { get; set; }
        public DateTime FechaEmisionCertificado { get; set; }
        public DateTime FechaVencimientoCertificado { get; set; }
        public string LugarEmision { get; set; }

        // Datos bancarios
        public string CBU { get; set; }
        public string TipoCuenta { get; set; }
        public string Banco { get; set; }

        // ? AGREGADA: Observaciones
        public string Observaciones { get; set; }
    }

}
