using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class HistorialCambioDto
    {
        public DateTime FechaCambio { get; set; }
        public string EstadoAnterior { get; set; }
        public string EstadoNuevo { get; set; }
        public string Usuario { get; set; }
        public string Comentario { get; set; }
    }

}
