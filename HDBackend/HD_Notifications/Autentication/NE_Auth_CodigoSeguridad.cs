using HD.AccesoDatos;

namespace HD.Notifications.Autentication
{
    public class NE_Auth_CodigoSeguridad
    {
        public static async Task<string> enviar(string _para, string _codigo)
        {
            try
            {
                List<string> para = new List<string>() { _para };
                string bodyhtml = body(_codigo);
                await NEEnviar.Click("Codigo de Autenticación", "humayadigital@humaya.com.mx", "!HD_Hum4y4D1g1t4l*T1?", bodyhtml, para.ToArray());
                return "Correo enviado con exito";
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        static string body(string _codigo)
        {
            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo.png");
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
                "<div style=\"margin-bottom:100px;\">\n" +
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
                                                "INICIO DE SESION \n" +
                                            "</div>\n" +
                                        "</td>\n" +
                                    "</tr>\n" +
                                "</table>\n" +
                            "</td>\n" +
                        "</tr>\n" +
                    "</table>\n" +
                "</div>\n" +

            //"<h1 style=\"font-size:18;\"><Font Color='#235B34'>" + datos_Correo.detail.tipo_credito + "</Font></h1></P>\n" +

            "<table class=\"tabla-documentacion-vencida\">\n" +
                "<thead>\n" +
                    "<tr>\n" +
                        "<th class=\"celda-cliente-titulo\">\n" +
                           "<div style=\"font-size:18px;\">" + "CODIGO DE AUTENTICACION" + "</div>\n" +
                        "</th>\n" +
                    "</tr>\n" +
                "</thead>\n" +
               "<tbody>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;text-align:center;\">\n" +
                            _codigo +
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
