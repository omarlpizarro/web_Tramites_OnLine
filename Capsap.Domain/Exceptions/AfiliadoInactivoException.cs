using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando un afiliado no está activo
    /// </summary>
    public class AfiliadoInactivoException : DomainException
    {
        public int AfiliadoId { get; }
        public string MatriculaProfesional { get; }

        public AfiliadoInactivoException(int afiliadoId)
            : base(
                "El afiliado no está activo en el sistema. No puede realizar solicitudes.",
                "AFILIADO_INACTIVO")
        {
            AfiliadoId = afiliadoId;
            AddData("AfiliadoId", afiliadoId);
        }

        public AfiliadoInactivoException(int afiliadoId, string matriculaProfesional)
            : base(
                $"El afiliado con matrícula {matriculaProfesional} no está activo en el sistema. No puede realizar solicitudes.",
                "AFILIADO_INACTIVO")
        {
            AfiliadoId = afiliadoId;
            MatriculaProfesional = matriculaProfesional;
            AddData("AfiliadoId", afiliadoId);
            AddData("MatriculaProfesional", matriculaProfesional);
        }
    }

}
