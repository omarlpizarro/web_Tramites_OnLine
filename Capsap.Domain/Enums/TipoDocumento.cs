using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Enums
{
    public enum TipoDocumento
    {
        // Comunes
        DNISolicitante = 1,
        DNIConyuge = 2,
        ConstanciaCBU = 3,

        // Matrimonio
        ActaMatrimonio = 10,

        // Maternidad/Nacimiento
        ActaNacimiento = 20,
        DNIRecienNacido = 21,
        SentenciaAdopcion = 22,
        SentenciaFiliacion = 23,

        // Hijo Discapacitado
        CertificadoDiscapacidad = 30,
        CertificadoSupervivencia = 31,

        // Otros
        AutorizacionTransferencia = 40,
        Otros = 99
    }

}
