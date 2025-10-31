using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    // ==========================================
    // EXCEPCIONES DE AFILIADO
    // ==========================================

    /// <summary>
    /// Excepción lanzada cuando un afiliado tiene deuda pendiente (Art. 73 Ley 4764/94)
    /// </summary>
    public class AfiliadoConDeudaException : DomainException
    {
        public int AfiliadoId { get; }
        public string MatriculaProfesional { get; }
        public decimal? MontoDeuda { get; }

        public AfiliadoConDeudaException(int afiliadoId)
            : base(
                "El afiliado tiene deuda pendiente con la institución. Debe regularizar su situación antes de solicitar beneficios. (Art. 73 Ley 4764/94)",
                "AFILIADO_CON_DEUDA")
        {
            AfiliadoId = afiliadoId;
            AddData("AfiliadoId", afiliadoId);
        }

        public AfiliadoConDeudaException(int afiliadoId, string matriculaProfesional, decimal montoDeuda)
            : base(
                $"El afiliado con matrícula {matriculaProfesional} tiene una deuda pendiente de ${montoDeuda:N2}. Debe regularizar su situación antes de solicitar beneficios. (Art. 73 Ley 4764/94)",
                "AFILIADO_CON_DEUDA")
        {
            AfiliadoId = afiliadoId;
            MatriculaProfesional = matriculaProfesional;
            MontoDeuda = montoDeuda;
            AddData("AfiliadoId", afiliadoId);
            AddData("MatriculaProfesional", matriculaProfesional);
            AddData("MontoDeuda", montoDeuda);
        }
    }

}
