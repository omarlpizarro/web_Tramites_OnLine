using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Helpers
{
    /// <summary>
    /// Helper para generación de tokens
    /// </summary>
    public static class TokenHelper
    {
        public static string GenerarToken(int longitud = 32)
        {
            using var rng = RandomNumberGenerator.Create();
            var tokenData = new byte[longitud];
            rng.GetBytes(tokenData);
            return Convert.ToBase64String(tokenData);
        }

        public static string GenerarTokenNumerico(int longitud = 6)
        {
            using var rng = RandomNumberGenerator.Create();
            var buffer = new byte[sizeof(uint)];
            var resultado = new StringBuilder();

            while (longitud-- > 0)
            {
                rng.GetBytes(buffer);
                var num = BitConverter.ToUInt32(buffer, 0);
                resultado.Append((num % 10).ToString());
            }

            return resultado.ToString();
        }
    }
}
