using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Helpers
{
    /// <summary>
    /// Helper para validaciones comunes
    /// </summary>
    public static class ValidationHelper
    {
        public static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool EsFechaValida(DateTime? fecha, DateTime? minima = null, DateTime? maxima = null)
        {
            if (!fecha.HasValue)
                return false;

            if (minima.HasValue && fecha.Value < minima.Value)
                return false;

            if (maxima.HasValue && fecha.Value > maxima.Value)
                return false;

            return true;
        }
    }
}
