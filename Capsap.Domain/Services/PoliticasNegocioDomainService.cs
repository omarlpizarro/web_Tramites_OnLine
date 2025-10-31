using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using Capsap.Domain.Extensions;
using Capsap.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Services
{
    public class PoliticasNegocioDomainService : IPoliticasNegocioDomainService
    {
        public Result PuedeCrearMultiplesSolicitudes(Afiliado afiliado, TipoSubsidio tipo, IEnumerable<SolicitudSubsidio> solicitudesExistentes)
        {
            // Verificar si ya existe una solicitud activa del mismo tipo
            var solicitudesActivas = solicitudesExistentes
                .Where(s => s.AfiliadoSolicitanteId == afiliado.Id &&
                           s.TipoSubsidio == tipo &&
                           !s.EstaFinalizada() &&
                           s.Activo)
                .ToList();

            if (solicitudesActivas.Any())
            {
                return Result.Failure(
                    $"Ya existe una solicitud activa de tipo {tipo.ObtenerDescripcion()}. " +
                    "Debe esperar a que se resuelva antes de crear una nueva."
                );
            }

            // Verificar límite anual según tipo
            var limiteAnual = ObtenerLimiteSolicitudesAnuales(tipo);
            if (limiteAnual > 0)
            {
                var solicitudesEsteAnio = solicitudesExistentes
                    .Where(s => s.AfiliadoSolicitanteId == afiliado.Id &&
                               s.TipoSubsidio == tipo &&
                               s.FechaSolicitud.Year == DateTime.Now.Year)
                    .Count();

                if (solicitudesEsteAnio >= limiteAnual)
                {
                    return Result.Failure(
                        $"Ha alcanzado el límite de {limiteAnual} solicitud(es) de tipo {tipo.ObtenerDescripcion()} por año."
                    );
                }
            }

            return Result.Success();
        }

        public Result ValidarIntervaloEntreSolicitudes(Afiliado afiliado, TipoSubsidio tipo, IEnumerable<SolicitudSubsidio> solicitudesExistentes)
        {
            // Regla: Solo para hijo discapacitado (anual)
            if (tipo == TipoSubsidio.HijoDiscapacitado)
            {
                var ultimaSolicitud = solicitudesExistentes
                    .Where(s => s.AfiliadoSolicitanteId == afiliado.Id &&
                               s.TipoSubsidio == tipo &&
                               s.Estado == EstadoSolicitud.Aprobada)
                    .OrderByDescending(s => s.FechaSolicitud)
                    .FirstOrDefault();

                if (ultimaSolicitud != null)
                {
                    var diasDesdeUltimaSolicitud = (DateTime.Now - ultimaSolicitud.FechaSolicitud).Days;
                    var diasRequeridos = 365; // 1 año

                    if (diasDesdeUltimaSolicitud < diasRequeridos)
                    {
                        var diasFaltantes = diasRequeridos - diasDesdeUltimaSolicitud;
                        return Result.Failure(
                            $"Debe esperar {diasFaltantes} días más para solicitar nuevamente el subsidio por hijo discapacitado. " +
                            "Este subsidio se otorga con una frecuencia anual."
                        );
                    }
                }
            }

            return Result.Success();
        }

        public int ObtenerLimiteSolicitudesAnuales(TipoSubsidio tipo)
        {
            return tipo switch
            {
                TipoSubsidio.Matrimonio => 1, // Solo una vez
                TipoSubsidio.Maternidad => 0, // Sin límite (puede tener varios hijos)
                TipoSubsidio.NacimientoAdopcion => 0, // Sin límite
                TipoSubsidio.HijoDiscapacitado => 1, // Una vez por año por hijo
                _ => 0
            };
        }
    }
}
