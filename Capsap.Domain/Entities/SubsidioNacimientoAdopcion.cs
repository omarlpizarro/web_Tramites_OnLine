using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    // ==========================================
    // SUBSIDIO POR NACIMIENTO/ADOPCIÓN
    // ==========================================
    public partial class SubsidioNacimientoAdopcion : EntityBase
    {
        public int SolicitudSubsidioId { get; set; }
        public TipoEventoNacimiento TipoEvento { get; set; }

        public virtual SolicitudSubsidio Solicitud { get; set; }
        public virtual ICollection<HijoNacimientoAdopcion> Hijos { get; set; }
    }

    public class HijoNacimientoAdopcion : EntityBase
    {
        public int SubsidioNacimientoAdopcionId { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string ActaNumero { get; set; }
        public string Tomo { get; set; }
        public string Anio { get; set; }
        public string CiudadNacimiento { get; set; }
        public string ProvinciaNacimiento { get; set; }
        public string DNI { get; set; }
        public string CUIL { get; set; }

        // Adopción
        public DateTime? FechaSentenciaJudicial { get; set; }
        public string CiudadSentencia { get; set; }
        public string ProvinciaSentencia { get; set; }

        public virtual SubsidioNacimientoAdopcion SubsidioNacimiento { get; set; }
    }

}
