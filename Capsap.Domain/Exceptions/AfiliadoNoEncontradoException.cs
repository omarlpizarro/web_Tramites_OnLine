using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se encuentra un afiliado
    /// </summary>
    public class AfiliadoNoEncontradoException : DomainException
    {
        public int? AfiliadoId { get; }
        public string Criterio { get; }

        public AfiliadoNoEncontradoException(int afiliadoId)
            : base(
                $"No se encontró el afiliado con ID {afiliadoId}.",
                "AFILIADO_NO_ENCONTRADO")
        {
            AfiliadoId = afiliadoId;
            AddData("AfiliadoId", afiliadoId);
        }

        public AfiliadoNoEncontradoException(string criterio)
            : base(
                $"No se encontró el afiliado con el criterio: {criterio}.",
                "AFILIADO_NO_ENCONTRADO")
        {
            Criterio = criterio;
            AddData("Criterio", criterio);
        }
    }


}
