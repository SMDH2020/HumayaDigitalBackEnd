using HD_Auditoria.Modelos.Justificaciones;
using HD_Auditoria.Modelos.Programar_Inventario;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Notificacion_Correo
{
    public class EnvioFinalizacionInventario
    {
        public static string _Mensaje { get; private set; }

        /// <param name="datos_correo">Contiene la lista de destinatarios</param>
        /// <param name="folio">Datos del inventario (folio, fecha_limite_just, diferencias)</param>
        /// <param name="pdfAdjunto">Bytes del PDF generado previamente (null = sin adjunto)</param>
        /// <param name="nombreArchivoPdf">Nombre que tendrá el archivo en el correo</param>
        public static Task<bool> Enviar_Finalizacion(
            mdl_Notificar_Finalizacion_View datos_correo,
            string? folio,
            byte[] pdfAdjunto = null,
            string nombreArchivoPdf = "Reporte_Inventario.pdf")
        {
            try
            {
                string password = "!HD_Hum4y4D1g1t4l*T1?";
                string _correo = "HumayaDigital@humaya.com.mx";

                MailMessage objeto_mail = new MailMessage();
                SmtpClient client = new SmtpClient
                {
                    Port = 587,
                    Host = "correo.humaya.com.mx",
                    Timeout = 20000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(_correo, password),
                    EnableSsl = false
                };

                objeto_mail.From = new MailAddress(_correo);

                // ── Destinatarios desde la lista de correos ──────────────────
                foreach (mdl_Notificar_Correo notificacion in datos_correo.correos)
                {
                     objeto_mail.To.Add(new MailAddress(notificacion.Correo));
                }

                objeto_mail.To.Add("guadalupeolivas@humaya.com.mx");

                // ── Asunto ───────────────────────────────────────────────────
                objeto_mail.Subject = $"Inventario {folio} — Finalizado";

                // ── Cuerpo ───────────────────────────────────────────────────
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = BodyFinalizacion(folio);

                // ── Adjunto PDF ──────────────────────────────────────────────
                if (pdfAdjunto != null && pdfAdjunto.Length > 0)
                {
                    var stream = new MemoryStream(pdfAdjunto);
                    var attachment = new Attachment(stream, nombreArchivoPdf, "application/pdf");
                    objeto_mail.Attachments.Add(attachment);
                }

                client.Send(objeto_mail);

                // Liberar adjuntos después de enviar
                objeto_mail.Attachments.Dispose();

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return Task.FromResult(false);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // EnvioFinalizacionInventario.cs — solo BodyFinalizacion cambia

        static string BodyFinalizacion(string? folio)
        {
            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo.jpg");
            string logo64 = Convert.ToBase64String(logo);

            return $@"
        <HTML>
        <HEAD>
        <style>
          body  {{ font-family: Arial, sans-serif; background:#f4f4f4; margin:0; padding:0; }}
          .wrap {{ max-width:600px; margin:30px auto; background:#fff;
                   border:1px solid #ddd; border-radius:6px; overflow:hidden; }}
          .hdr  {{ background:#477c2c; padding:0; }}
          .hdr table {{ width:100%; border-collapse:collapse; }}
          .hdr td.logo {{ width:90px; background:#477c2c; padding:8px; vertical-align:middle; }}
          .hdr td.titulo {{ background:#477c2c; padding:16px 20px;
                            font-size:18px; color:#fff; font-weight:bold; vertical-align:middle; }}
          .linea-amarilla {{ height:4px; background:#fedb05; }}
          .body {{ padding:28px 32px; }}
          .folio {{ display:inline-block; background:#eef4e8; border:1px solid #477c2c;
                    border-radius:4px; padding:6px 16px; font-size:20px;
                    font-weight:bold; color:#275027; margin:12px 0; }}
          .nota  {{ font-size:13px; color:#555; margin-top:14px; line-height:1.6; }}
          .pie   {{ background:#f9f9f9; border-top:1px solid #e0e0e0;
                    padding:12px 32px; font-size:11px; color:#999; }}
        </style>
        </HEAD>
        <BODY>
        <div class='wrap'>
          <div class='hdr'>
            <table>
              <tr>
                <td class='logo'>
                  <img src='data:image/jpeg;base64,{logo64}' width='70' height='70' style='display:block;'/>
                </td>
                <td class='titulo'>INVENTARIO FINALIZADO</td>
              </tr>
            </table>
          </div>
          <div class='linea-amarilla'></div>

          <div class='body'>
            <p style='font-size:14px;color:#333;margin:0;'>
              Se ha concluido exitosamente el proceso de inventario con folio:
            </p>
            <div class='folio'>{folio}</div>
            <p class='nota'>
              Se adjunta a este correo el reporte en formato PDF con el detalle completo
              de las diferencias detectadas y las métricas del resultado del inventario.
            </p>
          </div>

          <div class='pie'>
            Generado automáticamente por Humaya Digital — {DateTime.Now:dd/MM/yyyy HH:mm}
          </div>
        </div>
        </BODY>
        </HTML>";
        }
    }
}