using HD.AccesoDatos;
using HD.Notifications.Modelos;
using HD_Cobranza.Modelos.CondonacionIntereses;

namespace HD.Notifications.Cobranza
{
    // Correo de solicitud / aprobacion / rechazo de condonacion de intereses.
    // Antes vivia en AD_Condonacion_Intereses.EnviarCorreoCondonacion con SMTP;
    // se movio aqui porque HD_Cobranza no puede referenciar a HD_Notifications.
    public static class NotificacionCondonacionIntereses
    {
        // tipoEvento: "solicitud" | "aprobada" | "rechazada"
        // correosCsv: correos separados por coma, ej. "a@x.com,b@x.com" (ya resueltos por el stored)
        public static async Task<bool> Enviar(
            mdl_Correo_M365 config,
            mdl_Condonacion_Por_Folio? condonacion,
            string folio,
            string tipoEvento,
            string? correosCsv,
            string? mensajeAdicional = null)
        {
            try
            {
                var destinatarios = (correosCsv ?? string.Empty)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                if (!destinatarios.Any())
                    return false;

                if (condonacion == null)
                    throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = "No se encontró la condonación." });

                string asunto = tipoEvento switch
                {
                    "aprobada" => $"Condonación {folio} aprobada",
                    "rechazada" => $"Condonación {folio} rechazada",
                    _ => $"Nueva solicitud de condonación {folio}"
                };

                await NEEnviarM365.Enviar(config, asunto, CuerpoCorreoCondonacion(condonacion, tipoEvento, mensajeAdicional), destinatarios);

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

        private static string CuerpoCorreoCondonacion(mdl_Condonacion_Por_Folio c, string tipoEvento, string? mensajeAdicional)
        {
            string estatusTexto = tipoEvento switch
            {
                "aprobada" => "Aprobada",
                "rechazada" => "Rechazada",
                _ => "Pendiente de autorización"
            };

            string colorEstatus = tipoEvento switch
            {
                "aprobada" => "#2e7d32",
                "rechazada" => "#c62828",
                _ => "#ef6c00"
            };

            string tituloEvento = tipoEvento switch
            {
                "aprobada" => "Condonación de interés aprobada",
                "rechazada" => "Condonación de interés rechazada",
                _ => "Nueva solicitud de condonación de interés"
            };

            string comentariosHtml = string.IsNullOrWhiteSpace(c.Comentarios)
                ? ""
                : $@"<p style=""margin-top:15px;""><strong>Comentarios:</strong> {c.Comentarios}</p>";

            string mensajeAdicionalHtml = string.IsNullOrWhiteSpace(mensajeAdicional)
                ? ""
                : $@"<p style=""margin-top:15px;"">{mensajeAdicional}</p>";

            return $@"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""UTF-8"">
                <title>Condonación de interés</title>
            </head>
            <body style=""margin:0; padding:0; font-family: Arial, sans-serif; background-color: #f4f4f4;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""padding: 20px;"">
                    <tr><td align=""center"">
                        <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff; border:1px solid #cccccc; border-radius:8px;"">
                            <tr>
                                <td style=""padding:20px;"">
                                    <h2 style=""margin-top:0;"">{tituloEvento}</h2>
                                    <p style=""margin:0 0 10px 0;"">
                                        <strong>Folio:</strong> {c.Folio} &nbsp;|&nbsp;
                                        <strong>Estatus:</strong> <span style=""color:{colorEstatus}; font-weight:bold;"">{estatusTexto}</span>
                                    </p>
 
                                    <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""margin:10px 0;"">
                                        <tr>
                                            <td style=""background-color:#498c47; height:3px; border-radius:0 10px 10px 0; width:45%;""></td>
                                            <td style=""background-color:#498c47; height:3px; border-radius:10px 0 0 10px; width:45%;""></td>
                                        </tr>
                                    </table>
 
                                    <table width=""100%"" cellpadding=""6"" cellspacing=""0"" border=""0"" style=""font-size:14px;"">
                                        <tr><td><strong>Cliente:</strong></td><td>{c.razon_social}</td></tr>
                                        <tr><td><strong>Fecha:</strong></td><td>{c.createdate:dd/MM/yyyy HH:mm}</td></tr>
                                        <tr><td><strong>Solicita:</strong></td><td>{c.creador}</td></tr>
                                        <tr><td><strong>Saldo total:</strong></td><td>${c.Saldo:N2}</td></tr>
                                    </table>
 
                                    <h3 style=""margin-bottom:5px;"">Desglose por tipo de interés</h3>
                                    <table width=""100%"" cellpadding=""6"" cellspacing=""0"" border=""1"" style=""border-collapse:collapse; font-size:14px;"">
                                        <tr style=""background-color:#f0f0f0;"">
                                            <td><strong>Tipo</strong></td>
                                            <td><strong>Saldo</strong></td>
                                            <td><strong>% Condonado</strong></td>
                                            <td><strong>Descuento</strong></td>
                                            <td><strong>Interés pagado</strong></td>
                                        </tr>
                                        <tr>
                                            <td>Normal</td>
                                            <td>${c.Inormal_saldo:N2}</td>
                                            <td>{c.Inormal_porcentaje}%</td>
                                            <td>${c.Inormal_descuento:N2}</td>
                                            <td>${(c.Inormal_saldo - c.Inormal_descuento):N2}</td>
                                        </tr>
                                        <tr>
                                            <td>Moratorio</td>
                                            <td>${c.Imoratorio_saldo:N2}</td>
                                            <td>{c.Imoratorio_porcentaje}%</td>
                                            <td>${c.Imoratorio_descuento:N2}</td>
                                            <td>${(c.Imoratorio_saldo - c.Imoratorio_descuento):N2}</td>
                                        </tr>
                                    </table>
 
                                    {comentariosHtml}
                                    {mensajeAdicionalHtml}
                                </td>
                            </tr>
                        </table>
                    </td></tr>
                </table>
            </body>
            </html>";
        }
    }
}
