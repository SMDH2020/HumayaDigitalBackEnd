using HD.AccesoDatos;
using HD.Clientes.Consultas.SolicitudCreditoDocumento;
using HD.Clientes.Modelos.CRM.Cotizaciones;
using HD.Clientes.Reportes;
using HD.Notifications.Modelos;

namespace HD.Notifications.CRM
{
    // Envio por correo de la cotizacion CRM con el PDF adjunto.
    // Antes vivia en AD_Cotizaciones_CRM.EnviarPorCorreo con SMTP; se movio aqui
    // porque HD_Clientes no puede referenciar a HD_Notifications.
    public static class NotificacionCotizacionCRM
    {
        // Genera el PDF de la cotizacion (mismo generador que usa ImprimirPDF)
        // y lo envia por correo, adjunto, a los destinatarios indicados.
        // "vista" es el resultado de AD_Cotizaciones_CRM.ObtenerPorFolio.
        public static async Task<bool> Enviar(mdl_Correo_M365 config, mdl_Cotizaciones_CRM_Folio_View vista, string plantilla, IEnumerable<string> destinatarios, string mensajeAdicional)
        {
            try
            {
                if (vista?.Cotizacion == null)
                    throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = "No se encontró la cotización." });

                var correosValidos = (destinatarios ?? Enumerable.Empty<string>())
                    .Where(d => !string.IsNullOrWhiteSpace(d))
                    .Select(d => d.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                if (!correosValidos.Any())
                    throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = "Debes indicar al menos un destinatario." });

                RPT_Result documento = RPT_Cotizacion_CRM.GenerarPDF(vista, plantilla);
                byte[] pdfBytes = Convert.FromBase64String(documento.documento);

                // Se reutiliza el mismo Mapear que usa GenerarPDF (público) para
                // poder armar el cuerpo del correo con los mismos datos del PDF.
                var c = RPT_Cotizacion_CRM.Mapear(vista);

                string asunto = $"Cotización {c.folio_crm}" +
                    (string.IsNullOrWhiteSpace(c.asunto) ? "" : $" - {c.asunto}");

                var adjuntos = new List<mdl_Correo_Adjunto>
                {
                    new mdl_Correo_Adjunto
                    {
                        Nombre = $"{documento.nombredocumento}_{c.folio_crm}.pdf",
                        ContentType = "application/pdf",
                        Contenido = pdfBytes
                    }
                };

                await NEEnviarM365.Enviar(config, asunto, CuerpoCorreoCotizacion(c, mensajeAdicional), correosValidos, null, adjuntos);
                return true;
            }
            catch (Excepciones)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        private static string CuerpoCorreoCotizacion(mdl_Cotizacion_CRM_Imprimir c, string mensajeAdicional)
        {
            string mensaje = string.IsNullOrWhiteSpace(mensajeAdicional)
                ? "Adjunto encontrarás la cotización solicitada."
                : mensajeAdicional;

            return "<html><body style=\"font-family:Calibri, Arial, sans-serif; font-size:14px; color:#333;\">" +
                   $"<p>Estimado(a) <strong>{c.apreciable}</strong>,</p>" +
                   $"<p>{mensaje}</p>" +
                   "<p>" +
                   $"<strong>Folio de cotización:</strong> {c.folio_crm}<br/>" +
                   (string.IsNullOrWhiteSpace(c.asunto) ? "" : $"<strong>Asunto:</strong> {c.asunto}<br/>") +
                   $"<strong>Vigencia:</strong> {c.vigencia}" +
                   "</p>" +
                   "<p>Cualquier duda quedamos a tus órdenes.</p>" +
                   $"<p>Saludos,<br/>{c.asesorventa}<br/>Maquinaria del Humaya, S.A. de C.V.</p>" +
                   "</body></html>";
        }
    }
}
