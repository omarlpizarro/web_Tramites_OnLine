using Application.Interfaces;
using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DocumentoRepository : IDocumentoRepository
    {
        private readonly CapsapDbContext _context;

        public DocumentoRepository(CapsapDbContext context)
        {
            _context = context;
        }

        public async Task<DocumentoAdjunto?> ObtenerPorIdAsync(int id)
        {
            return await _context.DocumentosAdjuntos
                .Include(d => d.Solicitud)
                .Include(d => d.CertificadoPor)
                .FirstOrDefaultAsync(d => d.Id == id && d.Activo);
        }

        public async Task<List<DocumentoAdjunto>> ObtenerPorSolicitudAsync(int solicitudId)
        {
            return await _context.DocumentosAdjuntos
                .Include(d => d.CertificadoPor)
                .Where(d => d.SolicitudSubsidioId == solicitudId && d.Activo)
                .OrderBy(d => d.FechaCarga)
                .ToListAsync();
        }

        public async Task<List<DocumentoAdjunto>> ObtenerPorTipoAsync(int solicitudId, TipoDocumento tipoDocumento)
        {
            return await _context.DocumentosAdjuntos
                .Where(d => d.SolicitudSubsidioId == solicitudId &&
                           d.TipoDocumento == tipoDocumento &&
                           d.Activo)
                .OrderBy(d => d.FechaCarga)
                .ToListAsync();
        }

        public async Task<List<DocumentoAdjunto>> ObtenerCertificadosAsync(int solicitudId)
        {
            return await _context.DocumentosAdjuntos
                .Include(d => d.CertificadoPor)
                .Where(d => d.SolicitudSubsidioId == solicitudId &&
                           d.Certificado &&
                           d.Activo)
                .OrderBy(d => d.FechaCertificacion)
                .ToListAsync();
        }

        public async Task<List<DocumentoAdjunto>> ObtenerNoCertificadosAsync(int solicitudId)
        {
            return await _context.DocumentosAdjuntos
                .Where(d => d.SolicitudSubsidioId == solicitudId &&
                           !d.Certificado &&
                           d.Activo)
                .OrderBy(d => d.FechaCarga)
                .ToListAsync();
        }

        public async Task<DocumentoAdjunto?> ObtenerPorRutaAsync(string rutaArchivo)
        {
            return await _context.DocumentosAdjuntos
                .FirstOrDefaultAsync(d => d.RutaArchivo == rutaArchivo && d.Activo);
        }

        public async Task<bool> ExisteDocumentoTipoAsync(int solicitudId, TipoDocumento tipoDocumento)
        {
            return await _context.DocumentosAdjuntos
                .AnyAsync(d => d.SolicitudSubsidioId == solicitudId &&
                              d.TipoDocumento == tipoDocumento &&
                              d.Activo);
        }

        public async Task<DocumentoAdjunto> AgregarAsync(DocumentoAdjunto documento)
        {
            await _context.DocumentosAdjuntos.AddAsync(documento);
            await _context.SaveChangesAsync();
            return documento;
        }

        public async Task ActualizarAsync(DocumentoAdjunto documento)
        {
            _context.DocumentosAdjuntos.Update(documento);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var documento = await _context.DocumentosAdjuntos.FindAsync(id);
            if (documento != null)
            {
                documento.Activo = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> ContarPorSolicitudAsync(int solicitudId)
        {
            return await _context.DocumentosAdjuntos
                .CountAsync(d => d.SolicitudSubsidioId == solicitudId && d.Activo);
        }

        public async Task<int> ContarCertificadosAsync(int solicitudId)
        {
            return await _context.DocumentosAdjuntos
                .CountAsync(d => d.SolicitudSubsidioId == solicitudId &&
                                d.Certificado &&
                                d.Activo);
        }

        public async Task<long> ObtenerTamanoTotalAsync(int solicitudId)
        {
            return await _context.DocumentosAdjuntos
                .Where(d => d.SolicitudSubsidioId == solicitudId && d.Activo)
                .SumAsync(d => d.TamanoBytes);
        }

        public async Task<List<DocumentoAdjunto>> ObtenerDocumentosRecientesAsync(int cantidad = 10)
        {
            return await _context.DocumentosAdjuntos
                .Include(d => d.Solicitud)
                    .ThenInclude(s => s.AfiliadoSolicitante)
                .Where(d => d.Activo)
                .OrderByDescending(d => d.FechaCarga)
                .Take(cantidad)
                .ToListAsync();
        }

        public async Task<bool> TieneTodosDocumentosRequeridosAsync(int solicitudId, TipoSubsidio tipoSubsidio)
        {
            var documentosRequeridos = ObtenerDocumentosRequeridosPorTipo(tipoSubsidio);

            foreach (var tipoDoc in documentosRequeridos)
            {
                var existe = await ExisteDocumentoTipoAsync(solicitudId, tipoDoc);
                if (!existe)
                    return false;
            }

            return true;
        }

        private List<TipoDocumento> ObtenerDocumentosRequeridosPorTipo(TipoSubsidio tipoSubsidio)
        {
            return tipoSubsidio switch
            {
                TipoSubsidio.Matrimonio => new List<TipoDocumento>
            {
                TipoDocumento.ActaMatrimonio,
                TipoDocumento.DNISolicitante
            },
                TipoSubsidio.Maternidad => new List<TipoDocumento>
            {
                //TipoDocumento.CertificadoMedico,
                TipoDocumento.DNISolicitante
            },
                TipoSubsidio.NacimientoAdopcion => new List<TipoDocumento>
            {
                TipoDocumento.ActaNacimiento,
                TipoDocumento.DNISolicitante
            },
                TipoSubsidio.HijoDiscapacitado => new List<TipoDocumento>
            {
                TipoDocumento.CertificadoDiscapacidad,
                TipoDocumento.DNISolicitante
                //TipoDocumento.DNIBeneficiario
            },
                _ => new List<TipoDocumento> { TipoDocumento.DNISolicitante }
            };
        }
    }
}
