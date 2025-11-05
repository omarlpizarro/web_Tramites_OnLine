using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class AfiliadoDto
    {
        public int Id { get; set; }
        public string MatriculaProfesional { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string NombreCompleto { get; set; }
        public string DNI { get; set; }
        public string CUIL { get; set; }
        public EstadoCivil EstadoCivil { get; set; }
        public string EstadoCivilDescripcion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Domicilio { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }
        public bool TieneDeuda { get; set; }
        public bool Activo { get; set; }
    }
}
