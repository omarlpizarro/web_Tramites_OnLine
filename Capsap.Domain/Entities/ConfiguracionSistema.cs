using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    public class ConfiguracionSistema : EntityBase
    {
        public string Clave { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public string TipoDato { get; set; } // string, int, decimal, bool, json
    }
}
