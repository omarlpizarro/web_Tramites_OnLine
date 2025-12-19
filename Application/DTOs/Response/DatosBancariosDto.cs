using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class DatosBancariosDto
    {
        public string CBU { get; set; }
        public string CBUFormateado { get; set; }
        public string TipoCuenta { get; set; }
        public string Banco { get; set; }
        public bool TransferenciaATercero { get; set; }
        public string TitularCuenta { get; set; }
        public string CUITTitular { get; set; }
    }

}
