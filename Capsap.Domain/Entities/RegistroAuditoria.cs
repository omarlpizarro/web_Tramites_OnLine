using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Entities
{
    public class RegistroAuditoria
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string Entidad { get; set; }
        public int EntidadId { get; set; }
        public string Detalles { get; set; }
        public DateTime FechaHora { get; set; }
        public string DireccionIP { get; set; }
    }
}
