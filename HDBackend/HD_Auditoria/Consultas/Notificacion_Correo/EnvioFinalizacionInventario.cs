using HD_Auditoria.Modelos.Justificaciones;
using HD_Auditoria.Modelos.Programar_Inventario;
using HD.Notifications;
using HD.Notifications.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static async Task<bool> Enviar_Finalizacion(
            mdl_Correo_M365 config,
            mdl_Notificar_Finalizacion_View datos_correo,
            string? folio,
            byte[] pdfAdjunto = null,
            string nombreArchivoPdf = "Reporte_Inventario.pdf")
        {
            try
            {
                // ── Destinatarios desde la lista de correos ──────────────────
                string[] para = datos_correo.correos
                    .Select(n => n.Correo)
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Select(c => c!.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();
                //objeto_mail.To.Add("desarrolladorti@humaya.com.mx");
                //objeto_mail.To.Add("guadalupeolivas@humaya.com.mx");

                // ── Asunto ───────────────────────────────────────────────────
                string asunto = $"Inventario {folio} — Finalizado";

                // ── Cuerpo ───────────────────────────────────────────────────
                string cuerpo = BodyFinalizacion(folio);

                // ── Adjunto PDF ──────────────────────────────────────────────
                var adjuntos = new List<mdl_Correo_Adjunto>();
                if (pdfAdjunto != null && pdfAdjunto.Length > 0)
                {
                    adjuntos.Add(new mdl_Correo_Adjunto
                    {
                        Nombre = nombreArchivoPdf,
                        ContentType = "application/pdf",
                        Contenido = pdfAdjunto
                    });
                }

                await NEEnviarM365.Enviar(config, asunto, cuerpo, para, null, adjuntos);

                return true;
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
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
                            <p style='margin-top:24px;'>
                              Acceda al módulo desde el siguiente enlace:
                            </p>
                            <a class='btn' href='https://humayadigital.com/Auditoria/Justificaciones'>
                              Revisar Justificaciones
                            </a>

            Generado automáticamente por Humaya Digital — {DateTime.Now:dd/MM/yyyy HH:mm}
          </div>
        </div>
        </BODY>
        </HTML>";
        }
    }
}