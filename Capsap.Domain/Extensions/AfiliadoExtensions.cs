using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Extensions
{
    // ==========================================
    // EXTENSION METHODS PARA AFILIADOS
    // ==========================================
    public static class AfiliadoExtensions
    {
        public static string NombreCompleto(this Afiliado afiliado)
        {
            return $"{afiliado.Apellido}, {afiliado.Nombre}";
        }

        public static bool PuedeSolicitarSubsidios(this Afiliado afiliado)
        {
            return afiliado.Activo && !afiliado.TieneDeuda;
        }

        public static int Edad(this Afiliado afiliado, DateTime? fechaNacimiento)
        {
            if (!fechaNacimiento.HasValue)
                return 0;

            var hoy = DateTime.Today;
            var edad = hoy.Year - fechaNacimiento.Value.Year;
            if (fechaNacimiento.Value.Date > hoy.AddYears(-edad))
                edad--;
            return edad;
        }
    }

}
