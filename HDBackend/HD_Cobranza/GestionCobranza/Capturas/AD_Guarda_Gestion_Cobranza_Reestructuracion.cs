using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;
using System.Net.Mail;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Guarda_Gestion_Cobranza_Reestructuracion
    {
        private string CadenaConexion;
        public AD_Guarda_Gestion_Cobranza_Reestructuracion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Gestion_Cobranza_Reestructuracion>> Guardar(mdl_Gestion_Cobranza_Reestructuracion mdl, string email, string password)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @folio = mdl.folio,
                    @idcliente = mdl.idcliente,
                    @monto = mdl.monto,
                    @tipo_credito = mdl.tipo_credito,
                    @fecha_convenio = mdl.fecha_convenio,
                    @recordatorio = mdl.recordatorio,
                    @fecha_recordatorio = mdl.fecha_recordatorio,
                    @mediocontacto = mdl.mediocontacto,
                    @firma = mdl.firma,
                    @idresponsable = mdl.idresponsable,
                    @descuento = mdl.descuento,
                    @razon_descuento = mdl.razon_descuento,
                    @usuario = mdl.usuario,
                    @detalle = mdl.detalle,
                    @gestion = mdl.gestion,
                    @objecion = mdl.objecion,
                    @comentarios = mdl.comentarios,
                    @volvercontactar = mdl.volvercontactar,
                    @fechavolveracontactar = mdl.fechavolveracontactar,
                    @saldo = mdl.saldo,
                    @moratorios = mdl.moratorios,
                    @interespactado = mdl.interespactado,
                    @total = mdl.total,
                    @comentario_reestructuracion = mdl.comentario_reestructuracion,
                    @documento_reestructuracion = mdl.documento_reestructuracion,
                    @extension = mdl.extension
                };

                var result = await factory.SQL.QueryAsync<mdl_Gestion_Cobranza_Reestructuracion>("GestionCobranza.sp_Guardar_Gestion_Cobranza", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                enviarcorreo(mdl.documento_reestructuracion, mdl.idcliente, email, password, mdl.comentario_reestructuracion);
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<string> enviarcorreo(string _documento, int _idcliente, string email, string password, string comentarios)
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

                await Click($"Reestructuración del cliente {cliente}", email, password, bodyhtml, para.ToArray(), _documento, "Reestructuracion.pdf");
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

        public static Task<string> Click(string _asunto, string _correo, string _password, string _body, string[] para, string base64Documento = null, string nombreArchivo = "documento.pdf")
        {
            try
            {

                MailMessage objeto_mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "correo.humaya.com.mx";
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(_correo, _password);
                objeto_mail.From = new MailAddress(_correo);
                foreach (string to in para)
                {
                    objeto_mail.To.Add(new MailAddress(to));
                }
                objeto_mail.Subject = _asunto;
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = _body;
                client.EnableSsl = false;
                objeto_mail.Priority = MailPriority.Normal;
                if (!string.IsNullOrEmpty(base64Documento))
                {
                    byte[] fileBytes = Convert.FromBase64String(base64Documento);
                    MemoryStream ms = new MemoryStream(fileBytes);
                    Attachment attachment = new Attachment(ms, nombreArchivo, "application/pdf");
                    objeto_mail.Attachments.Add(attachment);
                }

                client.Send(objeto_mail);
                client.Send(objeto_mail);
                return Task.FromResult("Mensaje enviado con exito");
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }

        }
    }
}
