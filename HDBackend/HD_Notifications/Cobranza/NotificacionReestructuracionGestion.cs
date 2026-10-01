using Dapper;
using HD.AccesoDatos;
using HD.Notifications.Modelos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD.Notifications.Cobranza
{
    // Correo de reestructuracion de gestion de cobranza con el documento adjunto.
    // Antes vivia en AD_Guarda_Gestion_Cobranza_Reestructuracion.enviarcorreo con SMTP
    // (la llamada seguia comentada); se movio aqui porque HD_Cobranza no puede
    // referenciar a HD_Notifications.
    public static class NotificacionReestructuracionGestion
    {
        public static async Task<string> Enviar(mdl_Correo_M365 config, string CadenaConexion, string _documento, int _idcliente, string comentarios)
        {
            try
            {
                if (_documento.Contains(","))
                    _documento = _documento.Split(',')[1];

                _documento = _documento.Trim().Replace(" ", "+");

                byte[] fileBytes = Convert.FromBase64String(_documento);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = _idcliente
                };

                var result = await factory.SQL.QueryAsync<mdl_Gestion_Cobranza_Reestructuracion>("GestionCobranza.sp_Get_ADR_Clientes", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                List<string> para;

                if (result != null)
                {
                    if (result.FirstOrDefault().ADR == 1)
                    {
                        //para = new List<string> { "desarrolladorti2@humaya.com.mx", "guadalupeolivas@humaya.com.mx" };
                        para = new List<string> { "creditosinaloa@humaya.com.mx", "gerenciacobranza@humaya.com.mx", "cobranzasinaloa@humaya.com.mx", "martinzazueta@humaya.com.mx" };

                    }
                    else
                    {
                        //para = new List<string> { "desarrolladorti2@humaya.com.mx", "guadalupeolivas@humaya.com.mx" };
                        para = new List<string> { "creditonayarit@humaya.com.mx", "gerenciacobranza@humaya.com.mx", "cobranzanayarit@humaya.com.mx", "martinzazueta@humaya.com.mx" };
                    }
                }
                else
                {
                    return "Hubo un problema al obtener la región del cliente";
                }


                string cliente = result.FirstOrDefault().razon_social;
                //List<string> para = new List<string>() { "desarrolladorti2@humaya.com.mx" };
                string bodyhtml = body(cliente, comentarios);

                var adjuntos = new List<mdl_Correo_Adjunto>
                {
                    new mdl_Correo_Adjunto
                    {
                        Nombre = "Reestructuracion.pdf",
                        ContentType = "application/pdf",
                        Contenido = fileBytes
                    }
                };

                await NEEnviarM365.Enviar(config, $"Reestructuración del cliente {cliente}", bodyhtml, para.ToArray(), null, adjuntos);
                return "Correo enviado con exito";
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        static string body(string _cliente, string _comentarios)
        {
            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo.jpg");
            string logo64 = Convert.ToBase64String(logo);
            String sHtml;
            sHtml = "<HTML>\n" +
               "<HEAD>\n" +
               "<TITLE>REESTRUCTURACIÓN</TITLE>\n" +
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
                "<div style=\"margin-bottom:20px;\">\n" +
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
                                                "REESTRUCTURACION \n" +
                                            "</div>\n" +
                                        "</td>\n" +
                                    "</tr>\n" +
                                "</table>\n" +
                            "</td>\n" +
                        "</tr>\n" +
                    "</table>\n" +
                "</div>\n" +

            "<table class=\"tabla-documentacion-vencida\">\n" +
                "<thead>\n" +
                    "<tr>\n" +
                        "<th class=\"celda-cliente-titulo\">\n" +
                           "<div style=\"font-size:18px;\">" + "COMENTARIOS" + "</div>\n" +
                        "</th>\n" +
                    "</tr>\n" +
                "</thead>\n" +
               "<tbody>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px; text-align:justify;\">\n" +
                            _comentarios +
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
