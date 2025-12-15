using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public static class RolesConstantes
    {
        public const string Administrador = "Administrador";
        public const string Empleado = "Empleado";
        public const string Afiliado = "Afiliado";

        public const string AdminOEmpleado = "Administrador,Empleado";
        public const string Todos = "Administrador,Empleado,Afiliado";
    }
}
