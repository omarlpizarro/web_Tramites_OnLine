using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    // ==========================================
    // DTOs PAGINADOS
    // ==========================================

    public class PaginatedListDto<T>
    {
        public List<T> Items { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
        public int RegistrosPorPagina { get; set; }
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;

        public PaginatedListDto()
        {
            Items = new List<T>();
        }

        public PaginatedListDto(List<T> items, int totalRegistros, int paginaActual, int registrosPorPagina)
        {
            Items = items;
            TotalRegistros = totalRegistros;
            PaginaActual = paginaActual;
            RegistrosPorPagina = registrosPorPagina;
            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)registrosPorPagina);
        }
    }
}
