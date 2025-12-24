using Capsap.Domain.Enums;
using Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            _smtpUser = _configuration["Email:SmtpUser"] ?? "";
            _smtpPassword = _configuration["Email:SmtpPassword"] ?? "";
            _fromEmail = _configuration["Email:FromEmail"] ?? "noreply@capsap.gob.ar";
            _fromName = _configuration["Email:FromName"] ?? "CAPSAP - Caja de Previsión Social";
        }

        public async Task<bool> EnviarEmailAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true)
        {
            try
            {
                using var message = new MailMessage();
                message.From = new MailAddress(_fromEmail, _fromName);
                message.To.Add(new MailAddress(destinatario));
                message.Subject = asunto;
                message.Body = cuerpo;
                message.IsBodyHtml = esHtml;

                using var smtpClient = new SmtpClient(_smtpHost, _smtpPort);
                smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error al enviar email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EnviarConfirmacionSolicitudAsync(string emailAfiliado, string numeroSolicitud, string nombreAfiliado)
        {
            var asunto = $"Confirmación de Solicitud - {numeroSolicitud}";
            var cuerpo = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>Confirmación de Solicitud de Subsidio</h2>
                <p>Estimado/a {nombreAfiliado},</p>
                <p>Su solicitud de subsidio ha sido recibida exitosamente.</p>
                <p><strong>Número de solicitud:</strong> {numeroSolicitud}</p>
                <p>Puede hacer seguimiento de su solicitud ingresando a nuestro portal con su número de solicitud.</p>
                <p>Recibirá notificaciones por correo electrónico sobre el estado de su trámite.</p>
                <br>
                <p>Atentamente,</p>
                <p><strong>CAPSAP - Caja de Previsión Social de Abogados de la Provincia de Buenos Aires</strong></p>
            </body>
            </html>";

            return await EnviarEmailAsync(emailAfiliado, asunto, cuerpo);
        }

        public async Task<bool> EnviarNotificacionCambioEstadoAsync(string emailAfiliado, string numeroSolicitud, EstadoSolicitud nuevoEstado, string? comentario = null)
        {
            var estadoTexto = nuevoEstado switch
            {
                EstadoSolicitud.Aprobada => "APROBADA",
                EstadoSolicitud.Rechazada => "RECHAZADA",
                EstadoSolicitud.EnRevision => "EN REVISIÓN",
                EstadoSolicitud.DocumentacionIncompleta => "REQUIERE DOCUMENTACIÓN ADICIONAL",
                EstadoSolicitud.Pagada => "PAGADA",
                _ => nuevoEstado.ToString()
            };

            var asunto = $"Actualización de Solicitud {numeroSolicitud} - {estadoTexto}";

            var cuerpoComentario = !string.IsNullOrWhiteSpace(comentario)
                ? $"<p><strong>Observaciones:</strong> {comentario}</p>"
                : "";

            var cuerpo = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>Actualización del Estado de su Solicitud</h2>
                <p>Su solicitud <strong>{numeroSolicitud}</strong> ha cambiado de estado.</p>
                <p><strong>Nuevo estado:</strong> {estadoTexto}</p>
                {cuerpoComentario}
                <p>Para más detalles, ingrese a nuestro portal web.</p>
                <br>
                <p>Atentamente,</p>
                <p><strong>CAPSAP</strong></p>
            </body>
            </html>";

            return await EnviarEmailAsync(emailAfiliado, asunto, cuerpo);
        }

        public async Task<bool> EnviarSolicitudDocumentacionAsync(string emailAfiliado, string numeroSolicitud, List<string> documentosFaltantes)
        {
            var asunto = $"Documentación Requerida - Solicitud {numeroSolicitud}";

            var listaDocumentos = string.Join("", documentosFaltantes.Select(d => $"<li>{d}</li>"));

            var cuerpo = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>Documentación Adicional Requerida</h2>
                <p>Para continuar con el procesamiento de su solicitud <strong>{numeroSolicitud}</strong>, necesitamos que presente la siguiente documentación:</p>
                <ul>
                    {listaDocumentos}
                </ul>
                <p>Por favor, ingrese al portal y adjunte los documentos solicitados a la brevedad.</p>
                <br>
                <p>Atentamente,</p>
                <p><strong>CAPSAP</strong></p>
            </body>
            </html>";

            return await EnviarEmailAsync(emailAfiliado, asunto, cuerpo);
        }

        public async Task<bool> EnviarRecordatorioDocumentacionAsync(string emailAfiliado, string numeroSolicitud)
        {
            var asunto = $"Recordatorio: Documentación Pendiente - {numeroSolicitud}";

            var cuerpo = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>Recordatorio de Documentación Pendiente</h2>
                <p>Le recordamos que su solicitud <strong>{numeroSolicitud}</strong> se encuentra pendiente de documentación.</p>
                <p>Por favor, ingrese al portal y complete la documentación requerida para continuar con el proceso.</p>
                <br>
                <p>Atentamente,</p>
                <p><strong>CAPSAP</strong></p>
            </body>
            </html>";

            return await EnviarEmailAsync(emailAfiliado, asunto, cuerpo);
        }

        public async Task<bool> EnviarNotificacionAprobacionAsync(string emailAfiliado, string numeroSolicitud, decimal monto)
        {
            var asunto = $"Subsidio Aprobado - {numeroSolicitud}";

            var cuerpo = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>¡Su Subsidio ha sido Aprobado!</h2>
                <p>Nos complace informarle que su solicitud <strong>{numeroSolicitud}</strong> ha sido aprobada.</p>
                <p><strong>Monto aprobado:</strong> ${monto:N2}</p>
                <p>El pago se realizará en los próximos días hábiles a la cuenta bancaria declarada.</p>
                <br>
                <p>Atentamente,</p>
                <p><strong>CAPSAP</strong></p>
            </body>
            </html>";

            return await EnviarEmailAsync(emailAfiliado, asunto, cuerpo);
        }

        public async Task<bool> EnviarNotificacionRechazoAsync(string emailAfiliado, string numeroSolicitud, string motivo)
        {
            var asunto = $"Solicitud Rechazada - {numeroSolicitud}";

            var cuerpo = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>Resolución de Solicitud</h2>
                <p>Lamentamos informarle que su solicitud <strong>{numeroSolicitud}</strong> no ha sido aprobada.</p>
                <p><strong>Motivo:</strong> {motivo}</p>
                <p>Si tiene consultas o desea más información, puede comunicarse con nuestras oficinas.</p>
                <br>
                <p>Atentamente,</p>
                <p><strong>CAPSAP</strong></p>
            </body>
            </html>";

            return await EnviarEmailAsync(emailAfiliado, asunto, cuerpo);
        }

        public async Task<bool> EnviarEmailConAdjuntoAsync(string destinatario, string asunto, string cuerpo, string rutaAdjunto, bool esHtml = true)
        {
            try
            {
                using var message = new MailMessage();
                message.From = new MailAddress(_fromEmail, _fromName);
                message.To.Add(new MailAddress(destinatario));
                message.Subject = asunto;
                message.Body = cuerpo;
                message.IsBodyHtml = esHtml;

                if (File.Exists(rutaAdjunto))
                {
                    var attachment = new Attachment(rutaAdjunto);
                    message.Attachments.Add(attachment);
                }

                using var smtpClient = new SmtpClient(_smtpHost, _smtpPort);
                smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar email con adjunto: {ex.Message}");
                return false;
            }
        }
    }
}
