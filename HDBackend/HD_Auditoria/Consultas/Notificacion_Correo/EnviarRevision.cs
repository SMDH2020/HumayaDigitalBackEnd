using HD_Auditoria.Modelos.Justificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Notificacion_Correo
{
    public class EnviarRevision
    {
        public static string _Mensaje { get; private set; }

        public static Task<bool> Enviar_Almacen(IEnumerable<mdl_Notificar_Correo> mdl, string? folio)
        {
            try
            {
                string password = "!HD_Hum4y4D1g1t4l*T1?";
                string _correo = "HumayaDigital@humaya.com.mx";
                MailMessage objeto_mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "correo.humaya.com.mx";
                client.Timeout = 20000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(_correo, password);
                objeto_mail.From = new MailAddress(_correo);
                foreach (mdl_Notificar_Correo notificacion in mdl)
                {
                    objeto_mail.To.Add(new MailAddress(notificacion.Correo));
                }

                //objeto_mail.To.Add("desarrolladorti@humaya.com.mx");

                objeto_mail.Subject = "Justificaciones de inventario con folio: " + folio;
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = bodyAceptado(folio);
                client.EnableSsl = false;
                client.Send(objeto_mail);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return Task.FromResult(false);
            }

        }

        public static Task<bool> Enviar_Auditor(IEnumerable<mdl_Notificar_Correo> mdl, string? folio)
        {
            try
            {
                string password = "!HD_Hum4y4D1g1t4l*T1?";
                string _correo = "HumayaDigital@humaya.com.mx";
                MailMessage objeto_mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "correo.humaya.com.mx";
                client.Timeout = 20000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(_correo, password);
                objeto_mail.From = new MailAddress(_correo);
                foreach (mdl_Notificar_Correo notificacion in mdl)
                {
                    objeto_mail.To.Add(new MailAddress(notificacion.Correo));
                }

                //objeto_mail.To.Add("desarrolladorti@humaya.com.mx");

                objeto_mail.Subject = "Justificaciones de inventario con folio: " + folio;
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = bodyEvaluado(folio);
                client.EnableSsl = false;
                client.Send(objeto_mail);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return Task.FromResult(false);
            }

        }

        static string bodyAceptado(string folio)
        {
            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo.jpg");
            string logo64 = Convert.ToBase64String(logo);

            return $@"
                        <HTML>
                        <HEAD>
                        <TITLE>Justificación de Inventario</TITLE>
                        <style>
                          body  {{ font-family: Arial, sans-serif; background:#f4f4f4; margin:0; padding:0; }}
                          .wrap {{ max-width:620px; margin:30px auto; background:#fff;
                                   border:1px solid #ddd; border-radius:6px; overflow:hidden; }}
                          .linea-amarilla {{ height:4px; background:#fedb05; }}
                          .body {{ padding:30px 36px; color:#333; font-size:14px; line-height:1.7; }}
                          .folio-tag {{ display:inline-block; background:#eef4e8; border:1px solid #477c2c;
                                        border-radius:4px; padding:5px 14px; font-size:15px;
                                        font-weight:bold; color:#275027; margin:10px 0 18px; }}
                          .btn {{ display:inline-block; margin-top:22px; padding:11px 28px;
                                  background:#477c2c; color:#fff !important; font-size:14px;
                                  font-weight:bold; text-decoration:none; border-radius:5px;
                                  border-bottom:3px solid #275027; }}
                          .pie {{ background:#f9f9f9; border-top:1px solid #e0e0e0;
                                  padding:12px 36px; font-size:11px; color:#999; }}
                        </style>
                        </HEAD>
                        <BODY>
                        <div class='wrap'>

                          <!-- Encabezado -->
                          <table width='100%' cellspacing='0' cellpadding='0'>
                            <tr>
                              <td width='90' style='background:#477c2c; padding:8px; vertical-align:middle;'>
                                <img src='data:image/jpeg;base64,{logo64}' width='74' height='74' style='display:block;'/>
                              </td>
                              <td style='background:#477c2c; padding:16px 22px;
                                         font-size:19px; color:#fff; font-weight:bold; vertical-align:middle;'>
                                REVISIÓN DE JUSTIFICACIONES
                              </td>
                            </tr>
                          </table>
                          <div class='linea-amarilla'></div>

                          <!-- Cuerpo -->
                          <div class='body'>
                            <p>Estimado equipo,</p>
                            <p>
                              Le informamos que todas las diferencias correspondientes al inventario con folio:
                            </p>
                            <div class='folio-tag'>{folio}</div>
                            <p>
                              han sido <strong>justificadas en su totalidad</strong>. Le solicitamos amablemente
                              revisar las justificaciones registradas y validar que la información sea correcta
                              antes de proceder con el cierre definitivo del inventario.
                            </p>
                            <p>
                              En caso de requerir alguna corrección o encontrar alguna inconsistencia,
                              le pedimos realizarla a la brevedad posible a través del módulo de justificaciones.
                            </p>

                            <p style='margin-top:24px;'>
                              Puede acceder directamente desde el siguiente enlace:
                            </p>
                            <a class='btn' href='https://humayadigital.com/Auditoria/Justificaciones'>
                              Ver Justificaciones
                            </a>
                          </div>

                          <!-- Pie -->
                          <div class='pie'>
                            Este mensaje fue generado automáticamente por Humaya Digital.
                            Por favor no responda directamente a este correo.
                            &nbsp;·&nbsp; {DateTime.Now:dd/MM/yyyy HH:mm}
                          </div>

                        </div>
                        </BODY>
                        </HTML>";
        }

        static string bodyEvaluado(string folio)
        {
            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo.jpg");
            string logo64 = Convert.ToBase64String(logo);

            return $@"
                        <HTML>
                        <HEAD>
                        <TITLE>Evaluación de Justificaciones</TITLE>
                        <style>
                          body  {{ font-family: Arial, sans-serif; background:#f4f4f4; margin:0; padding:0; }}
                          .wrap {{ max-width:620px; margin:30px auto; background:#fff;
                                   border:1px solid #ddd; border-radius:6px; overflow:hidden; }}
                          .linea-amarilla {{ height:4px; background:#fedb05; }}
                          .body {{ padding:30px 36px; color:#333; font-size:14px; line-height:1.7; }}
                          .folio-tag {{ display:inline-block; background:#eef4e8; border:1px solid #477c2c;
                                        border-radius:4px; padding:5px 14px; font-size:15px;
                                        font-weight:bold; color:#275027; margin:10px 0 18px; }}
                          .aviso {{ background:#fff8e1; border-left:4px solid #fedb05;
                                    border-radius:4px; padding:12px 16px; margin:18px 0;
                                    font-size:13px; color:#5a4a00; }}
                          .btn {{ display:inline-block; margin-top:22px; padding:11px 28px;
                                  background:#477c2c; color:#fff !important; font-size:14px;
                                  font-weight:bold; text-decoration:none; border-radius:5px;
                                  border-bottom:3px solid #275027; }}
                          .pie {{ background:#f9f9f9; border-top:1px solid #e0e0e0;
                                  padding:12px 36px; font-size:11px; color:#999; }}
                        </style>
                        </HEAD>
                        <BODY>
                        <div class='wrap'>

                          <!-- Encabezado -->
                          <table width='100%' cellspacing='0' cellpadding='0'>
                            <tr>
                              <td width='90' style='background:#477c2c; padding:8px; vertical-align:middle;'>
                                <img src='data:image/jpeg;base64,{logo64}' width='74' height='74' style='display:block;'/>
                              </td>
                              <td style='background:#477c2c; padding:16px 22px;
                                         font-size:19px; color:#fff; font-weight:bold; vertical-align:middle;'>
                                RESULTADO DE JUSTIFICACIONES
                              </td>
                            </tr>
                          </table>
                          <div class='linea-amarilla'></div>

                          <!-- Cuerpo -->
                          <div class='body'>
                            <p>Estimado encargado de almacén,</p>
                            <p>
                              Le informamos que las justificaciones correspondientes al inventario con folio:
                            </p>
                            <div class='folio-tag'>{folio}</div>
                            <p>
                              han sido <strong>evaluadas en su totalidad</strong> por el equipo de auditoría.
                              Le solicitamos ingresar al módulo de justificaciones para revisar el resultado
                              de cada una de ellas.
                            </p>

                            <div class='aviso'>
                              ⚠️ En caso de contar con justificaciones <strong>rechazadas</strong>, es necesario
                              que las corrija y vuelva a justificar a la brevedad, ya que esto puede afectar
                              el cierre del inventario.
                            </div>

                            <p>
                              Si todas sus justificaciones fueron aprobadas, no es necesario realizar ninguna
                              acción adicional.
                            </p>

                            <p style='margin-top:24px;'>
                              Acceda al módulo desde el siguiente enlace:
                            </p>
                            <a class='btn' href='https://humayadigital.com/Auditoria/Justificaciones'>
                              Revisar Justificaciones
                            </a>
                          </div>

                          <!-- Pie -->
                          <div class='pie'>
                            Este mensaje fue generado automáticamente por Humaya Digital.
                            Por favor no responda directamente a este correo.
                            &nbsp;·&nbsp; {DateTime.Now:dd/MM/yyyy HH:mm}
                          </div>

                        </div>
                        </BODY>
                        </HTML>";
        }
    }
}
