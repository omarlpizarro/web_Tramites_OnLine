using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.ValueObjects
{
    public class MatriculaProfesional
    {
        public string Numero { get; private set; }

        private MatriculaProfesional(string numero)
        {
            Numero = numero;
        }

        public static Result<MatriculaProfesional> Crear(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
            {
                return Result<MatriculaProfesional>.Failure("El número de matrícula es requerido");
            }

            // Validar formato (ajustar según necesidad)
            if (numero.Length < 3 || numero.Length > 20)
            {
                return Result<MatriculaProfesional>.Failure("El número de matrícula tiene un formato inválido");
            }

            return Result<MatriculaProfesional>.Success(new MatriculaProfesional(numero.Trim().ToUpper()));
        }

        public override string ToString()
        {
            return Numero;
        }
    }

}
