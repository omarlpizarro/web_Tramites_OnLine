using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class DatosHijoDto
    {
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string DNI { get; set; }
        public string CUIL { get; set; }

        // Acta de nacimiento
        public string ActaNumero { get; set; }
        public string Tomo { get; set; }
        public string Anio { get; set; }
        public string CiudadNacimiento { get; set; }
        public string ProvinciaNacimiento { get; set; }

        // Adopción/Filiación (si aplica)
        public DateTime? FechaSentenciaJudicial { get; set; }
        public string CiudadSentencia { get; set; }
        public string ProvinciaSentencia { get; set; }

    }

}
