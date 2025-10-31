using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    // ==========================================
    // SUBSIDIO POR HIJO DISCAPACITADO
    // ==========================================
    public class SubsidioHijoDiscapacitado : EntityBase
    {
        public int SolicitudSubsidioId { get; set; }
        public TipoSolicitudDiscapacidad TipoSolicitud { get; set; } // Primera vez o Anual

        // Datos del hijo
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string ActaNumero { get; set; }
        public string Tomo { get; set; }
        public string Anio { get; set; }
        public string CiudadNacimiento { get; set; }
        public string ProvinciaNacimiento { get; set; }
        public string DNI { get; set; }
        public string CUIL { get; set; }

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

        public virtual SolicitudSubsidio Solicitud { get; set; }
    }


}
