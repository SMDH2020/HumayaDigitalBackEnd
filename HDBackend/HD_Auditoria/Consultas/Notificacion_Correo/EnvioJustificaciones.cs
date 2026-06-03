using HD_Auditoria.Modelos.Justificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Notificacion_Correo
{
    public class EnvioJustificaciones
    {
        public static string _Mensaje { get; private set; }

        public static Task<bool> Enviar_Aceptado(mdl_Notificar_View datos_correo, mdl_Justificaciones_Acciones mdl)
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
                //foreach (mdl_Notificar_Correo notificacion in datos_correo.correos)
                //{
                //    objeto_mail.To.Add(new MailAddress(notificacion.Correo));
                //}

                objeto_mail.To.Add("desarrolladorti@humaya.com.mx");

                objeto_mail.Subject = "Justificaciones aceptadas correctamente de inventario con folio: " + mdl.folio;
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = bodyAceptado(mdl);
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

        public static Task<bool> Enviar_Rechazado(mdl_Notificar_View datos_correo, mdl_Justificaciones_Acciones mdl)
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
                foreach (mdl_Notificar_Correo notificacion in datos_correo.correos)
                {
                    objeto_mail.To.Add(new MailAddress(notificacion.Correo));
                }

                //objeto_mail.To.Add("desarrolladorti@humaya.com.mx");

                objeto_mail.Subject = "Justificación rechazada en inventario con folio: " + mdl.folio;
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = bodyRechazado(mdl);
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

        static string bodyAceptado(mdl_Justificaciones_Acciones datos_Correo)

        {

            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo.jpg");

            string logo64 = Convert.ToBase64String(logo);

            String sHtml;

            sHtml = "<HTML>\n" +
               "<HEAD>\n" +
               "<TITLE>SOLICITUD DE CREDITO</TITLE>\n" +
               "<style> \n" +
                ".text-container{ \n" +
                    "margin-top:50px; \n" +
                    "font-size:20px;\n" +
                    "text-align:justify;\n" +
                "}\n" +
                ".tabla-documentacion-vencida {\n" +
                    "border-collapse: collapse;\n" +
                    "width: 100%;\n" +
                    "border: 2px solid #275027;\n" +
                    "max-width:1200px;\n" +
                    "margin: 0 auto;\n" +
                    "border-spacing:0;\n" +
                "}\n" +

                    ".head-documentacion{\n" +
                        "background-color:#275027;\n" +
                        "color:#fff;\n" +
                        "border-bottom:3px solid #fedb05;\n" +
                    "}\n" +
                    ".celda-cliente-informacion{\n" +
                        "padding:4px;\n" +
                        "border-bottom:1px solid #afb69d;\n" +
                    "}\n" +
                    ".celda-cliente-titulo{\n" +
                        "padding:4px;\n" +
                        "border-bottom: 4px solid #fedb05;\n" +
                        "background-color:#275027;\n" +
                        "color:#fff;\n" +
                        "text-align:center;\n" +
                    "}\n" +
                "</style>\n" +
               "</HEAD>\n" +
               "<BODY style=\"text-align:center;\"><P>\n" +
               "<div style=\"margin-bottom:30px;\">\n" +
                    "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\n" +
                        "<tr>\n" +
                            "<td width=\"10%\" style=\"padding: 0;\"> \n" +
                                "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\n" +
                                    "<tr>\n" +
                                        "<td style=\"background-color: #477c2c;\" height=\"70\">\n" +
                                            "<div style=\"margin: 0 auto;\">\n" +
                                            "</div>\n" +
                                        "</td>\n" +
                                    "</tr>\n" +
                                "</table>\n" +
                            "</td>\n" +
                            "<td width=\"1%\" style=\"padding: 0;\">\n" +
                                "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\n" +
                                    "<tr>\n" +
                                        "<td style=\"padding: 0;\">\n" +
                                            "<div style=\"margin: 0;\">\n" +
                                                  "<img width=\"150\" height=\"150\" src='data:image/png;base64," + logo64 + "' style=\"display: block;\"/>\n" +
                                            "</div>\n" +
                                        "</td>\n" +
                                    "</tr>\n" +
                                "</table>\n" +
                            "</td>\n" +
                            "<td width=\"auto\" style=\"padding: 0;\">\n" +
                                "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\n" +
                                    "<tr>\n" +
                                        "<td style=\"display: flex; align-items: center;font-size:24px;color:#fff; background-color: #477c2c;\" height=\"70\">\n" +
                                            "<div style=\"margin-left: 50px; \">\n" +
                                                "SOLICITUD DE CREDITO \n" +
                                            "</div>\n" +
                                        "</td>\n" +
                                    "</tr>\n" +
                                "</table>\n" +
                            "</td>\n" +
                        "</tr>\n" +
                    "</table>\n" +
                "</div>\n" +
            "<table class=\"tabla-documentacion-vencida\" >\n" +
               "<tbody>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\" colspan=\"2\">\n" +
                        " FOLIO: " + datos_Correo.folio +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " Estatus: " + "Todas las justificaciones fueron aceptadas" +
                        "</td>\n" +
                    "</tr>\n" +
               "</tbody>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }

        static string bodyRechazado(mdl_Justificaciones_Acciones datos_Correo)

        {

            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo.jpg");

            string logo64 = Convert.ToBase64String(logo);

            String sHtml;

            sHtml = "<HTML>\n" +
               "<HEAD>\n" +
               "<TITLE>SOLICITUD DE CREDITO</TITLE>\n" +
               "<style> \n" +
                ".text-container{ \n" +
                    "margin-top:50px; \n" +
                    "font-size:20px;\n" +
                    "text-align:justify;\n" +
                "}\n" +
                ".tabla-documentacion-vencida {\n" +
                    "border-collapse: collapse;\n" +
                    "width: 100%;\n" +
                    "border: 2px solid #275027;\n" +
                    "max-width:1200px;\n" +
                    "margin: 0 auto;\n" +
                    "border-spacing:0;\n" +
                "}\n" +

                    ".head-documentacion{\n" +
                        "background-color:#275027;\n" +
                        "color:#fff;\n" +
                        "border-bottom:3px solid #fedb05;\n" +
                    "}\n" +
                    ".celda-cliente-informacion{\n" +
                        "padding:4px;\n" +
                        "border-bottom:1px solid #afb69d;\n" +
                    "}\n" +
                    ".celda-cliente-titulo{\n" +
                        "padding:4px;\n" +
                        "border-bottom: 4px solid #fedb05;\n" +
                        "background-color:#275027;\n" +
                        "color:#fff;\n" +
                        "text-align:center;\n" +
                    "}\n" +
                "</style>\n" +
               "</HEAD>\n" +
               "<BODY style=\"text-align:center;\"><P>\n" +
               "<div style=\"margin-bottom:30px;\">\n" +
                    "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\n" +
                        "<tr>\n" +
                            "<td width=\"10%\" style=\"padding: 0;\"> \n" +
                                "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\n" +
                                    "<tr>\n" +
                                        "<td style=\"background-color: #477c2c;\" height=\"70\">\n" +
                                            "<div style=\"margin: 0 auto;\">\n" +
                                            "</div>\n" +
                                        "</td>\n" +
                                    "</tr>\n" +
                                "</table>\n" +
                            "</td>\n" +
                            "<td width=\"1%\" style=\"padding: 0;\">\n" +
                                "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\n" +
                                    "<tr>\n" +
                                        "<td style=\"padding: 0;\">\n" +
                                            "<div style=\"margin: 0;\">\n" +
                                                  "<img width=\"150\" height=\"150\" src='data:image/png;base64," + logo64 + "' style=\"display: block;\"/>\n" +
                                            "</div>\n" +
                                        "</td>\n" +
                                    "</tr>\n" +
                                "</table>\n" +
                            "</td>\n" +
                            "<td width=\"auto\" style=\"padding: 0;\">\n" +
                                "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\n" +
                                    "<tr>\n" +
                                        "<td style=\"display: flex; align-items: center;font-size:24px;color:#fff; background-color: #477c2c;\" height=\"70\">\n" +
                                            "<div style=\"margin-left: 50px; \">\n" +
                                                "SOLICITUD DE CREDITO \n" +
                                            "</div>\n" +
                                        "</td>\n" +
                                    "</tr>\n" +
                                "</table>\n" +
                            "</td>\n" +
                        "</tr>\n" +
                    "</table>\n" +
                "</div>\n" +
            "<table class=\"tabla-documentacion-vencida\" >\n" +
               "<tbody>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\" colspan=\"2\">\n" +
                        " FOLIO: " + datos_Correo.folio +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " Estatus: " + "Fue rechazado una justificación y es necesario volver a cargar documentación" +
                        "</td>\n" +
                    "</tr>\n" +
               "</tbody>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }
    }
}
