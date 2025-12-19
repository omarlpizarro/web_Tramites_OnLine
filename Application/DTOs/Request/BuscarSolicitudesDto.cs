using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    // ==========================================
    // DTOs DE REQUEST - BÚSQUEDA
    // ==========================================

    public class BuscarSolicitudesDto
    {
        public string NumeroSolicitud { get; set; }
        public string MatriculaProfesional { get; set; }
        public EstadoSolicitud? Estado { get; set; }
        public TipoSubsidio? TipoSubsidio { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int PaginaActual { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 10;
    }
}
