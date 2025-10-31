using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Extensions
{
    // ==========================================
    // EXTENSION METHODS PARA String
    // ==========================================
    public static class StringExtensions
    {
        public static string Truncar(this string texto, int longitudMaxima, string sufijo = "...")
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            if (texto.Length <= longitudMaxima)
                return texto;

            return texto.Substring(0, longitudMaxima - sufijo.Length) + sufijo;
        }

        public static string QuitarEspaciosYGuiones(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            return texto.Replace(" ", "").Replace("-", "").Replace(".", "");
        }

        public static bool ContieneNumeros(this string texto)
        {
            return !string.IsNullOrEmpty(texto) && texto.Any(char.IsDigit);
        }

        public static bool SoloNumeros(this string texto)
        {
            return !string.IsNullOrEmpty(texto) && texto.All(char.IsDigit);
        }

        public static string PrimeraLetraMayuscula(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            return char.ToUpper(texto[0]) + texto.Substring(1).ToLower();
        }

        public static string FormatearCBU(this string cbu)
        {
            if (string.IsNullOrEmpty(cbu) || cbu.Length != 22)
                return cbu;

            // Formato: 1234567-8-12345678901234-5
            return $"{cbu.Substring(0, 7)}-{cbu[7]}-{cbu.Substring(8, 13)}-{cbu[21]}";
        }

        public static string FormatearCUIL(this string cuil)
        {
            if (string.IsNullOrEmpty(cuil) || cuil.Length != 11)
                return cuil;

            // Formato: 20-12345678-9
            return $"{cuil.Substring(0, 2)}-{cuil.Substring(2, 8)}-{cuil[10]}";
        }

        public static string FormatearDNI(this string dni)
        {
            if (string.IsNullOrEmpty(dni))
                return dni;

            dni = dni.QuitarEspaciosYGuiones();

            // Formato: 12.345.678
            if (dni.Length == 7)
                return $"{dni.Substring(0, 1)}.{dni.Substring(1, 3)}.{dni.Substring(4, 3)}";
            else if (dni.Length == 8)
                return $"{dni.Substring(0, 2)}.{dni.Substring(2, 3)}.{dni.Substring(5, 3)}";

            return dni;
        }
    }
}
