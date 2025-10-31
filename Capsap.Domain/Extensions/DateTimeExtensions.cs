using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Extensions
{
    // ==========================================
    // EXTENSION METHODS PARA DateTime
    // ==========================================
    public static class DateTimeExtensions
    {
        public static string FormatoArgentino(this DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy");
        }

        public static string FormatoArgentinoConHora(this DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy HH:mm");
        }

        public static string TiempoTranscurrido(this DateTime fecha)
        {
            var diferencia = DateTime.Now - fecha;

            if (diferencia.TotalMinutes < 1)
                return "Hace un momento";
            if (diferencia.TotalMinutes < 60)
                return $"Hace {(int)diferencia.TotalMinutes} minuto(s)";
            if (diferencia.TotalHours < 24)
                return $"Hace {(int)diferencia.TotalHours} hora(s)";
            if (diferencia.TotalDays < 30)
                return $"Hace {(int)diferencia.TotalDays} día(s)";
            if (diferencia.TotalDays < 365)
                return $"Hace {(int)(diferencia.TotalDays / 30)} mes(es)";

            return $"Hace {(int)(diferencia.TotalDays / 365)} año(s)";
        }

        public static bool EsFechaValida(this DateTime? fecha, DateTime? fechaMinima = null, DateTime? fechaMaxima = null)
        {
            if (!fecha.HasValue)
                return false;

            if (fechaMinima.HasValue && fecha.Value < fechaMinima.Value)
                return false;

            if (fechaMaxima.HasValue && fecha.Value > fechaMaxima.Value)
                return false;

            return true;
        }
    }
}
