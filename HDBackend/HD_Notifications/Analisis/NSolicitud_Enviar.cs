using HD.Clientes.Modelos;
using System.Net.Mail;

namespace HD.Notifications.Analisis
{
    public static class NSolicitud_Enviar
    {
        public static string _Mensaje { get; private set; }
        //         public static void Enviar(string Correo, string _tipoSolicitud, string _folio, string _vendedor, string _cliente, string _linea, string
        //_monto)
        public static Task<bool> Enviar(mdlSolicitudCredito_Enviar_View datos_correo)
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
                objeto_mail.To.Add(new MailAddress(datos_correo.mdlSolicitud.correo_responsable_credito));
                objeto_mail.To.Add(new MailAddress(datos_correo.mdlSolicitud.correo_gerente_sucursal));
                objeto_mail.To.Add(new MailAddress(datos_correo.mdlSolicitud.correo_vendedor));
                if (datos_correo.mdlSolicitud.correo_responsable_credito2 != null)
                {
                    objeto_mail.To.Add(new MailAddress(datos_correo.mdlSolicitud.correo_responsable_credito2));
                }
                if (datos_correo.mdlSolicitud.correo_responsable_credito3 != null)
                {
                    objeto_mail.To.Add(new MailAddress(datos_correo.mdlSolicitud.correo_responsable_credito3));
                }


                //objeto_mail.To.Add(new MailAddress("desarrolladorti@humaya.com.mx"));
                //objeto_mail.To.Add(new MailAddress("desarrolladorti2@humaya.com.mx"));


                objeto_mail.Subject = "Nueva solicitud de credito";
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = body(datos_correo);
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

        static string body(mdlSolicitudCredito_Enviar_View datos_Correo)

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
                "<div>\n" +
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
                "</div>\n"+

               "<h1 style=\"font-size:18;\"><Font Color='#235B34'>" + datos_Correo.detail.tipo_credito + "</Font></h1></P>\n" +

            "<table class=\"tabla-documentacion-vencida\">\n" +
                "<thead>\n" +
                    "<tr>\n" +
                        "<th colspan=\"2\" class=\"celda-cliente-titulo\">\n" +
                           "<div style=\"font-size:18px;\">" + datos_Correo.detail.tipo_credito + "</div>\n" +
                        "</th>\n" +
                    "</tr>\n" +
                "</thead>\n"+
               "<tbody>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\">\n" +
                        "FOLIO \n" +
                        "</td>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d;text-align:right;\">\n" +
                        datos_Correo.detail.folio +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\">\n" +
                        "VENDEDOR \n" +
                        "</td>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d;text-align:right\">\n" +
                        datos_Correo.detail.vendedor +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\">\n" +
                        "CLIENTE \n" +
                        "</td>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d; text-align:right\">\n" +
                        datos_Correo.detail.razon_social +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\">\n" +
                        "LINEA \n" +
                        "</td>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d;text-align:right\">\n" +
                        datos_Correo.detail.linea_credito +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\">\n" +
                        "MONTO \n" +
                        "</td>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d;text-align:right\">\n" +
                        datos_Correo.detail.importe.ToString("N2") +
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
