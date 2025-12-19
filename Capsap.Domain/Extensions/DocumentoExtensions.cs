using Capsap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Extensions
{
    // ==========================================
    // EXTENSION METHODS PARA DOCUMENTOS
    // ==========================================
    public static class DocumentoExtensions
    {
        public static string ObtenerExtension(this DocumentoAdjunto documento)
        {
            return Path.GetExtension(documento.NombreArchivo).ToLowerInvariant();
        }

        public static bool EsPDF(this DocumentoAdjunto documento)
        {
            return documento.ObtenerExtension() == ".pdf";
        }

        public static bool EsImagen(this DocumentoAdjunto documento)
        {
            var extensionesImagen = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return extensionesImagen.Contains(documento.ObtenerExtension());
        }

        public static string TamanoLegible(this DocumentoAdjunto documento)
        {
            return FormatearTamano(documento.TamanoBytes);
        }

        private static string FormatearTamano(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
