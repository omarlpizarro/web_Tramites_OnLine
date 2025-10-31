using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Constants
{
    public static class DomainConstants
    {
        // Plazos
        public const int PLAZO_MAXIMO_DIAS_MATRIMONIO = 180;
        public const int PLAZO_MAXIMO_DIAS_MATERNIDAD = 180;
        public const int PLAZO_MAXIMO_DIAS_NACIMIENTO = 180;

        // Límites de archivos
        public const long TAMANO_MAXIMO_ARCHIVO_BYTES = 10 * 1024 * 1024; // 10 MB
        public static readonly string[] EXTENSIONES_PERMITIDAS = { ".pdf", ".jpg", ".jpeg", ".png" };
        public static readonly string[] CONTENT_TYPES_PERMITIDOS = { "application/pdf", "image/jpeg", "image/png" };

        // Validaciones
        public const int LONGITUD_MINIMA_DNI = 7;
        public const int LONGITUD_MAXIMA_DNI = 8;
        public const int LONGITUD_CUIL = 11;
        public const int LONGITUD_CBU = 22;
        public const int LONGITUD_MINIMA_MATRICULA = 3;
        public const int LONGITUD_MAXIMA_MATRICULA = 20;

        // Mensajes de error comunes
        public static class ErrorMessages
        {
            public const string AFILIADO_CON_DEUDA = "El afiliado tiene deuda pendiente con la institución. Debe regularizar su situación antes de solicitar beneficios. (Art. 73 Ley 4764/94)";
            public const string SOLICITUD_FUERA_DE_PLAZO = "La solicitud está fuera de plazo. El plazo máximo es de {0} días corridos desde el evento.";
            public const string DOCUMENTO_REQUERIDO = "El documento {0} es requerido para este tipo de subsidio.";
            public const string ARCHIVO_MUY_GRANDE = "El archivo excede el tamaño máximo permitido de {0} MB.";
            public const string EXTENSION_NO_PERMITIDA = "La extensión del archivo no está permitida. Solo se aceptan: {0}.";
            public const string CBU_INVALIDO = "El CBU proporcionado no es válido.";
            public const string DNI_INVALIDO = "El DNI proporcionado no es válido.";
            public const string CUIL_INVALIDO = "El CUIL proporcionado no es válido.";
        }

        // Configuración de emails
        public static class EmailTemplates
        {
            public const string SOLICITUD_CREADA = "SolicitudCreada";
            public const string SOLICITUD_ENVIADA = "SolicitudEnviada";
            public const string SOLICITUD_APROBADA = "SolicitudAprobada";
            public const string SOLICITUD_RECHAZADA = "SolicitudRechazada";
            public const string DOCUMENTACION_INCOMPLETA = "DocumentacionIncompleta";
            public const string NUEVA_SOLICITUD_EMPLEADOS = "NuevaSolicitudEmpleados";
        }

        // Roles del sistema
        public static class Roles
        {
            public const string AFILIADO = "Afiliado";
            public const string EMPLEADO_MESA_ENTRADA = "EmpleadoMesaEntrada";
            public const string EMPLEADO_REVISOR = "EmpleadoRevisor";
            public const string ADMINISTRADOR = "Administrador";
        }

        // Claims personalizados
        public static class Claims
        {
            public const string AFILIADO_ID = "AfiliadoId";
            public const string MATRICULA_PROFESIONAL = "MatriculaProfesional";
            public const string TIENE_DEUDA = "TieneDeuda";
        }
    }

}
