using Capsap.Domain.Constants;
using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using Capsap.Domain.ValueObjects;
using Capsap.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    public class AfiliadoDomainService : IAfiliadoDomainService
    {
        public Result PuedeRealizarSolicitud(Afiliado afiliado)
        {
            if (!afiliado.Activo)
            {
                return Result.Failure("El afiliado no está activo en el sistema.");
            }

            if (afiliado.TieneDeuda)
            {
                return Result.Failure(DomainConstants.ErrorMessages.AFILIADO_CON_DEUDA);
            }

            return Result.Success();
        }

        public Result VerificarEstadoAfiliacion(Afiliado afiliado)
        {
            if (string.IsNullOrWhiteSpace(afiliado.MatriculaProfesional))
            {
                return Result.Failure("El afiliado no tiene matrícula profesional asignada.");
            }

            if (string.IsNullOrWhiteSpace(afiliado.Email))
            {
                return Result.Failure("El afiliado debe tener un email registrado.");
            }

            return Result.Success();
        }

        public bool TieneSolicitudesActivasDelMismoTipo(Afiliado afiliado, TipoSubsidio tipo, IEnumerable<SolicitudSubsidio> solicitudes)
        {
            return solicitudes.Any(s =>
                s.AfiliadoSolicitanteId == afiliado.Id &&
                s.TipoSubsidio == tipo &&
                !s.EstaFinalizada() &&
                s.Activo);
        }
    }

}
