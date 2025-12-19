using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.ValueObjects
{
    // ==========================================
    // VALUE OBJECT: Documento de Identidad
    // ==========================================
    public class DocumentoIdentidad
    {
        public TipoDocumentoIdentidad Tipo { get; private set; }
        public string Numero { get; private set; }

        private DocumentoIdentidad(TipoDocumentoIdentidad tipo, string numero)
        {
            Tipo = tipo;
            Numero = numero;
        }

        public static Result<DocumentoIdentidad> Crear(TipoDocumentoIdentidad tipo, string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
            {
                return Result<DocumentoIdentidad>.Failure("El número de documento es requerido");
            }

            // Limpiar el número (remover puntos y espacios)
            numero = numero.Replace(".", "").Replace(" ", "").Trim();

            // Validar según tipo
            var validacion = tipo switch
            {
                TipoDocumentoIdentidad.DNI => ValidarDNI(numero),
                TipoDocumentoIdentidad.CUIL => ValidarCUIL(numero),
                TipoDocumentoIdentidad.CUIT => ValidarCUIT(numero),
                _ => Result.Success()
            };

            if (!validacion.IsSuccess)
            {
                return Result<DocumentoIdentidad>.Failure(validacion.Error);
            }

            return Result<DocumentoIdentidad>.Success(new DocumentoIdentidad(tipo, numero));
        }

        private static Result ValidarDNI(string dni)
        {
            // DNI debe tener entre 7 y 8 dígitos
            if (dni.Length < 7 || dni.Length > 8)
            {
                return Result.Failure("El DNI debe tener entre 7 y 8 dígitos");
            }

            if (!dni.All(char.IsDigit))
            {
                return Result.Failure("El DNI solo puede contener números");
            }

            return Result.Success();
        }

        private static Result ValidarCUIL(string cuil)
        {
            // CUIL debe tener exactamente 11 dígitos
            if (cuil.Length != 11)
            {
                return Result.Failure("El CUIL debe tener exactamente 11 dígitos");
            }

            if (!cuil.All(char.IsDigit))
            {
                return Result.Failure("El CUIL solo puede contener números");
            }

            // Validar dígito verificador
            if (!ValidarDigitoVerificadorCUIL(cuil))
            {
                return Result.Failure("El CUIL ingresado no es válido (dígito verificador incorrecto)");
            }

            return Result.Success();
        }

        private static Result ValidarCUIT(string cuit)
        {
            // CUIT tiene la misma estructura que CUIL
            return ValidarCUIL(cuit);
        }

        private static bool ValidarDigitoVerificadorCUIL(string cuil)
        {
            try
            {
                int[] multiplicadores = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
                int suma = 0;

                for (int i = 0; i < 10; i++)
                {
                    suma += int.Parse(cuil[i].ToString()) * multiplicadores[i];
                }

                int resto = suma % 11;
                int digitoVerificador = resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;

                return digitoVerificador == int.Parse(cuil[10].ToString());
            }
            catch
            {
                return false;
            }
        }

        public string FormatoLegible()
        {
            return Tipo switch
            {
                TipoDocumentoIdentidad.DNI => FormatearDNI(Numero),
                TipoDocumentoIdentidad.CUIL => FormatearCUIL(Numero),
                TipoDocumentoIdentidad.CUIT => FormatearCUIL(Numero), // Mismo formato que CUIL
                _ => Numero
            };
        }

        private string FormatearDNI(string dni)
        {
            // Formato: 12.345.678
            if (dni.Length == 7)
            {
                return $"{dni.Substring(0, 1)}.{dni.Substring(1, 3)}.{dni.Substring(4, 3)}";
            }
            else if (dni.Length == 8)
            {
                return $"{dni.Substring(0, 2)}.{dni.Substring(2, 3)}.{dni.Substring(5, 3)}";
            }
            return dni;
        }

        private string FormatearCUIL(string cuil)
        {
            // Formato: 20-12345678-9
            return $"{cuil.Substring(0, 2)}-{cuil.Substring(2, 8)}-{cuil[10]}";
        }

        public override string ToString()
        {
            return Numero;
        }

        public override bool Equals(object obj)
        {
            if (obj is DocumentoIdentidad other)
            {
                return Tipo == other.Tipo && Numero == other.Numero;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tipo, Numero);
        }

    }

}
