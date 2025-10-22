using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.ValueObjects
{
    public class CBU
    {
        public string Valor { get; private set; }

        private CBU(string valor)
        {
            Valor = valor;
        }

        public static Result<CBU> Crear(string cbu)
        {
            if (!EsValido(cbu))
            {
                return Result<CBU>.Failure("El CBU proporcionado no es válido");
            }

            return Result<CBU>.Success(new CBU(cbu));
        }

        public static bool EsValido(string cbu)
        {
            if (string.IsNullOrWhiteSpace(cbu))
                return false;

            // Remover espacios y guiones
            cbu = cbu.Replace(" ", "").Replace("-", "");

            // Debe tener exactamente 22 dígitos
            if (cbu.Length != 22)
                return false;

            // Debe contener solo números
            if (!cbu.All(char.IsDigit))
                return false;

            // Validación de dígitos verificadores
            return ValidarDigitosVerificadores(cbu);
        }

        private static bool ValidarDigitosVerificadores(string cbu)
        {
            try
            {
                // Validar primer bloque (8 caracteres)
                var bloque1 = cbu.Substring(0, 7);
                var digitoVerificador1 = int.Parse(cbu[7].ToString());
                var calculado1 = CalcularDigitoVerificador(bloque1, new[] { 7, 1, 3, 9, 7, 1, 3 });

                if (digitoVerificador1 != calculado1)
                    return false;

                // Validar segundo bloque (14 caracteres)
                var bloque2 = cbu.Substring(8, 13);
                var digitoVerificador2 = int.Parse(cbu[21].ToString());
                var calculado2 = CalcularDigitoVerificador(bloque2, new[] { 3, 9, 7, 1, 3, 9, 7, 1, 3, 9, 7, 1, 3 });

                return digitoVerificador2 == calculado2;
            }
            catch
            {
                return false;
            }
        }

        private static int CalcularDigitoVerificador(string numero, int[] pesos)
        {
            int suma = 0;
            for (int i = 0; i < numero.Length; i++)
            {
                suma += int.Parse(numero[i].ToString()) * pesos[i];
            }

            int diferencia = 10 - (suma % 10);
            return diferencia == 10 ? 0 : diferencia;
        }

        public override string ToString()
        {
            return Valor;
        }

        public string FormatoLegible()
        {
            // Formato: 1234567-8-12345678901234-5
            return $"{Valor.Substring(0, 7)}-{Valor[7]}-{Valor.Substring(8, 13)}-{Valor[21]}";
        }
    }

}
