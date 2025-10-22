using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Enums
{
    public enum EstadoSolicitud
    {
        Borrador = 1,           // Iniciada pero no enviada
        Enviada = 2,            // Enviada por el afiliado
        EnRevision = 3,         // Siendo revisada por empleado
        DocumentacionIncompleta = 4, // Requiere documentación adicional
        Aprobada = 5,
        Rechazada = 6,
        Pagada = 7
    }

}
