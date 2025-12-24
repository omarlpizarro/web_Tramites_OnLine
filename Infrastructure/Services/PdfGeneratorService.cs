using Capsap.Domain.Entities;
using Capsap.Domain.Enums;
using Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        // NOTA: Esta es una implementación básica usando HTML a PDF
        // Para producción, considera usar librerías como:
        // - QuestPDF (recomendado, moderno y gratuito)
        // - iTextSharp (clásico pero de pago para uso comercial)
        // - PdfSharpCore (open source)

        public async Task<byte[]> GenerarComprobanteSolicitudAsync(SolicitudSubsidio solicitud)
        {
            var html = GenerarHtmlComprobante(solicitud);
            return await ConvertirHtmlAPdfAsync(html);
        }

        public async Task<byte[]> GenerarResolucionAsync(SolicitudSubsidio solicitud, string resolucion)
        {
            var html = GenerarHtmlResolucion(solicitud, resolucion);
            return await ConvertirHtmlAPdfAsync(html);
        }

        public async Task<byte[]> GenerarReporteSolicitudesAsync(List<SolicitudSubsidio> solicitudes, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var html = GenerarHtmlReporte(solicitudes, fechaDesde, fechaHasta);
            return await ConvertirHtmlAPdfAsync(html);
        }

        public async Task<byte[]> GenerarConstanciaDeudaAsync(Afiliado afiliado, bool tieneDeuda, decimal? montoDeuda = null)
        {
            var html = GenerarHtmlConstanciaDeuda(afiliado, tieneDeuda, montoDeuda);
            return await ConvertirHtmlAPdfAsync(html);
        }

        private string GenerarHtmlComprobante(SolicitudSubsidio solicitud)
        {
            var tipoSubsidioTexto = solicitud.TipoSubsidio switch
            {
                TipoSubsidio.Matrimonio => "Matrimonio",
                TipoSubsidio.Maternidad => "Maternidad",
                TipoSubsidio.NacimientoAdopcion => "Nacimiento/Adopción",
                TipoSubsidio.HijoDiscapacitado => "Hijo con Discapacidad",
                _ => solicitud.TipoSubsidio.ToString()
            };

            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Comprobante de Solicitud</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 40px;
            color: #333;
        }}
        .header {{
            text-align: center;
            border-bottom: 3px solid #003366;
            padding-bottom: 20px;
            margin-bottom: 30px;
        }}
        .header h1 {{
            color: #003366;
            margin: 0;
        }}
        .header p {{
            margin: 5px 0;
            color: #666;
        }}
        .info-section {{
            margin: 20px 0;
        }}
        .info-section h2 {{
            background-color: #003366;
            color: white;
            padding: 10px;
            margin: 0;
        }}
        .info-content {{
            border: 1px solid #ddd;
            padding: 15px;
        }}
        .info-row {{
            margin: 10px 0;
        }}
        .label {{
            font-weight: bold;
            display: inline-block;
            width: 180px;
        }}
        .footer {{
            margin-top: 50px;
            text-align: center;
            color: #666;
            font-size: 12px;
            border-top: 1px solid #ddd;
            padding-top: 20px;
        }}
        .qr-placeholder {{
            width: 100px;
            height: 100px;
            background-color: #f0f0f0;
            margin: 20px auto;
            text-align: center;
            line-height: 100px;
            border: 1px solid #ddd;
        }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>CAPSAP</h1>
        <p>Caja de Previsión Social de Abogados de la Provincia de Buenos Aires</p>
        <p>Ley Provincial 4764/94</p>
    </div>

    <div style='text-align: center; margin: 30px 0;'>
        <h2 style='color: #003366;'>COMPROBANTE DE SOLICITUD DE SUBSIDIO</h2>
    </div>

    <div class='info-section'>
        <h2>Información de la Solicitud</h2>
        <div class='info-content'>
            <div class='info-row'>
                <span class='label'>Número de Solicitud:</span>
                <span>{solicitud.NumeroSolicitud}</span>
            </div>
            <div class='info-row'>
                <span class='label'>Tipo de Subsidio:</span>
                <span>{tipoSubsidioTexto}</span>
            </div>
            <div class='info-row'>
                <span class='label'>Fecha de Solicitud:</span>
                <span>{solicitud.FechaSolicitud:dd/MM/yyyy HH:mm}</span>
            </div>
            <div class='info-row'>
                <span class='label'>Estado:</span>
                <span>{solicitud.Estado}</span>
            </div>
        </div>
    </div>

    <div class='info-section'>
        <h2>Datos del Solicitante</h2>
        <div class='info-content'>
            <div class='info-row'>
                <span class='label'>Nombre Completo:</span>
                <span>{solicitud.AfiliadoSolicitante.Apellido}, {solicitud.AfiliadoSolicitante.Nombre}</span>
            </div>
            <div class='info-row'>
                <span class='label'>Matrícula Profesional:</span>
                <span>{solicitud.AfiliadoSolicitante.MatriculaProfesional}</span>
            </div>
            <div class='info-row'>
                <span class='label'>DNI:</span>
                <span>{solicitud.AfiliadoSolicitante.DNI}</span>
            </div>
            <div class='info-row'>
                <span class='label'>CUIL:</span>
                <span>{solicitud.AfiliadoSolicitante.CUIL}</span>
            </div>
            <div class='info-row'>
                <span class='label'>Email:</span>
                <span>{solicitud.AfiliadoSolicitante.Email}</span>
            </div>
            <div class='info-row'>
                <span class='label'>Teléfono:</span>
                <span>{solicitud.AfiliadoSolicitante.Telefono}</span>
            </div>
        </div>
    </div>

    <div class='info-section'>
        <h2>Datos Bancarios</h2>
        <div class='info-content'>
            <div class='info-row'>
                <span class='label'>CBU:</span>
                <span>{solicitud.CBU}</span>
            </div>
            <div class='info-row'>
                <span class='label'>Banco:</span>
                <span>{solicitud.Banco}</span>
            </div>
            <div class='info-row'>
                <span class='label'>Tipo de Cuenta:</span>
                <span>{solicitud.TipoCuenta}</span>
            </div>
            <div class='info-row'>
                <span class='label'>Titular:</span>
                <span>{solicitud.TitularCuenta}</span>
            </div>
        </div>
    </div>

    <div style='margin: 30px 0; padding: 15px; background-color: #f9f9f9; border-left: 4px solid #003366;'>
        <p style='margin: 0;'><strong>Nota:</strong> Conserve este comprobante. Puede hacer seguimiento de su solicitud ingresando al portal con el número de solicitud.</p>
    </div>

    <div class='footer'>
        <p>Este es un documento generado automáticamente</p>
        <p>Fecha de emisión: {DateTime.Now:dd/MM/yyyy HH:mm}</p>
        <p>CAPSAP - Calle Ejemplo 123 - La Plata - Tel: (0221) XXX-XXXX</p>
    </div>
</body>
</html>";

            return html;
        }

        private string GenerarHtmlResolucion(SolicitudSubsidio solicitud, string resolucion)
        {
            var estadoTexto = solicitud.Estado == EstadoSolicitud.Aprobada ? "APROBADA" : "RECHAZADA";

            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Resolución</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 40px;
            color: #333;
        }}
        .header {{
            text-align: center;
            border-bottom: 3px solid #003366;
            padding-bottom: 20px;
            margin-bottom: 30px;
        }}
        .resolucion {{
            text-align: center;
            background-color: {(solicitud.Estado == EstadoSolicitud.Aprobada ? "#28a745" : "#dc3545")};
            color: white;
            padding: 20px;
            margin: 30px 0;
            font-size: 24px;
            font-weight: bold;
        }}
        .contenido {{
            line-height: 1.8;
            text-align: justify;
            margin: 30px 0;
        }}
        .firma {{
            margin-top: 80px;
            text-align: center;
        }}
        .linea-firma {{
            border-top: 1px solid #333;
            width: 300px;
            margin: 50px auto 10px auto;
        }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>CAPSAP</h1>
        <p>Caja de Previsión Social de Abogados de la Provincia de Buenos Aires</p>
    </div>

    <div class='resolucion'>
        RESOLUCIÓN: SOLICITUD {estadoTexto}
    </div>

    <p><strong>Expediente N°:</strong> {solicitud.NumeroSolicitud}</p>
    <p><strong>Fecha:</strong> {DateTime.Now:dd/MM/yyyy}</p>

    <div class='contenido'>
        <p>La Plata, {DateTime.Now:dd 'de' MMMM 'de' yyyy}</p>
        
        <p>VISTO la solicitud de subsidio presentada por {solicitud.AfiliadoSolicitante.Apellido}, {solicitud.AfiliadoSolicitante.Nombre}, 
        Matrícula Profesional N° {solicitud.AfiliadoSolicitante.MatriculaProfesional}, DNI N° {solicitud.AfiliadoSolicitante.DNI};</p>

        <p>Y CONSIDERANDO:</p>
        
        <p>{resolucion}</p>

        <p>Por ello, en uso de las facultades conferidas por la Ley 4764/94 y su reglamentación;</p>

        <p style='text-align: center; font-weight: bold; margin: 30px 0;'>SE RESUELVE:</p>

        <p><strong>ARTÍCULO 1°:</strong> {(solicitud.Estado == EstadoSolicitud.Aprobada
                ? $"APROBAR la solicitud de subsidio N° {solicitud.NumeroSolicitud}."
                : $"RECHAZAR la solicitud de subsidio N° {solicitud.NumeroSolicitud}.")}</p>

        <p><strong>ARTÍCULO 2°:</strong> Notifíquese al interesado.</p>

        <p><strong>ARTÍCULO 3°:</strong> Regístrese, comuníquese y archívese.</p>
    </div>

    <div class='firma'>
        <div class='linea-firma'></div>
        <p><strong>Presidente de CAPSAP</strong></p>
        <p>Ley 4764/94</p>
    </div>
</body>
</html>";

            return html;
        }

        private string GenerarHtmlReporte(List<SolicitudSubsidio> solicitudes, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var filasSolicitudes = new StringBuilder();

            foreach (var sol in solicitudes)
            {
                filasSolicitudes.AppendLine($@"
                <tr>
                    <td>{sol.NumeroSolicitud}</td>
                    <td>{sol.FechaSolicitud:dd/MM/yyyy}</td>
                    <td>{sol.AfiliadoSolicitante.Apellido}, {sol.AfiliadoSolicitante.Nombre}</td>
                    <td>{sol.TipoSubsidio}</td>
                    <td>{sol.Estado}</td>
                </tr>");
            }

            var periodo = fechaDesde.HasValue && fechaHasta.HasValue
                ? $"Período: {fechaDesde:dd/MM/yyyy} - {fechaHasta:dd/MM/yyyy}"
                : "Todas las solicitudes";

            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Reporte de Solicitudes</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 40px; }}
        .header {{ text-align: center; margin-bottom: 30px; }}
        table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
        th, td {{ border: 1px solid #ddd; padding: 12px; text-align: left; }}
        th {{ background-color: #003366; color: white; }}
        tr:nth-child(even) {{ background-color: #f9f9f9; }}
        .totales {{ margin-top: 30px; padding: 15px; background-color: #f0f0f0; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>REPORTE DE SOLICITUDES DE SUBSIDIOS</h1>
        <p>{periodo}</p>
        <p>Generado: {DateTime.Now:dd/MM/yyyy HH:mm}</p>
    </div>

    <table>
        <thead>
            <tr>
                <th>N° Solicitud</th>
                <th>Fecha</th>
                <th>Afiliado</th>
                <th>Tipo</th>
                <th>Estado</th>
            </tr>
        </thead>
        <tbody>
            {filasSolicitudes}
        </tbody>
    </table>

    <div class='totales'>
        <h3>Resumen</h3>
        <p><strong>Total de solicitudes:</strong> {solicitudes.Count}</p>
        <p><strong>Aprobadas:</strong> {solicitudes.Count(s => s.Estado == EstadoSolicitud.Aprobada)}</p>
        <p><strong>Rechazadas:</strong> {solicitudes.Count(s => s.Estado == EstadoSolicitud.Rechazada)}</p>
        <p><strong>Pendientes:</strong> {solicitudes.Count(s => s.Estado == EstadoSolicitud.EnRevision)}</p>
    </div>
</body>
</html>";

            return html;
        }

        private string GenerarHtmlConstanciaDeuda(Afiliado afiliado, bool tieneDeuda, decimal? montoDeuda)
        {
            var estadoDeuda = tieneDeuda ? "REGISTRA DEUDA" : "NO REGISTRA DEUDA";
            var colorEstado = tieneDeuda ? "#dc3545" : "#28a745";

            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Constancia de Deuda</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 40px; }}
        .header {{ text-align: center; border-bottom: 3px solid #003366; padding-bottom: 20px; }}
        .estado {{ 
            text-align: center; 
            background-color: {colorEstado}; 
            color: white; 
            padding: 20px; 
            margin: 30px 0; 
            font-size: 24px; 
        }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>CAPSAP</h1>
        <p>Caja de Previsión Social de Abogados de la Provincia de Buenos Aires</p>
    </div>

    <h2 style='text-align: center;'>CONSTANCIA DE DEUDA</h2>

    <div class='estado'>{estadoDeuda}</div>

    <p><strong>Apellido y Nombre:</strong> {afiliado.Apellido}, {afiliado.Nombre}</p>
    <p><strong>Matrícula:</strong> {afiliado.MatriculaProfesional}</p>
    <p><strong>DNI:</strong> {afiliado.DNI}</p>
    <p><strong>CUIL:</strong> {afiliado.CUIL}</p>
    
    {(tieneDeuda && montoDeuda.HasValue ? $"<p><strong>Monto de Deuda:</strong> ${montoDeuda:N2}</p>" : "")}

    <p style='margin-top: 50px;'><strong>Fecha de emisión:</strong> {DateTime.Now:dd/MM/yyyy}</p>
    
    <p style='margin-top: 30px; font-size: 12px; color: #666;'>
        Esta constancia es válida por 30 días desde su emisión y certifica el estado de deuda del afiliado 
        según lo establecido en el Art. 73 de la Ley 4764/94.
    </p>
</body>
</html>";

            return html;
        }

        private async Task<byte[]> ConvertirHtmlAPdfAsync(string html)
        {
            // IMPLEMENTACIÓN BÁSICA: Solo retorna el HTML como bytes
            // Para producción, implementar conversión real con una librería de PDF

            // Opción 1: Usar QuestPDF (recomendado)
            // Opción 2: Usar iTextSharp
            // Opción 3: Usar PuppeteerSharp para renderizar HTML a PDF

            // Por ahora, retornamos el HTML codificado
            return await Task.FromResult(Encoding.UTF8.GetBytes(html));
        }
    }
}
