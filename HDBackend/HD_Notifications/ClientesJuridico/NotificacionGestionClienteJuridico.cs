using HD.Clientes.Modelos.SC_Analisis;
using HD.Notifications.Modelos;
using HD_Cobranza.Modelos.Juridico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.ClientesJuridico
{
    public class NotificacionGestionClienteJuridico
    {
        public static string _Mensaje { get; private set; }
        //         public static void Enviar(string Correo, string _tipoSolicitud, string _folio, string _vendedor, string _cliente, string _linea, string
        //_monto)
        public static async Task<bool> Enviar(mdl_Correo_M365 config, mdl_Clientes_Juridico_Correo datos_correo)
        {
            try
            {
                // Destinatarios: el/los correos del usuario indicado + copia de pruebas a desarrolladorti
                var destinatarios = (datos_correo.correo ?? string.Empty)
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToList();
                destinatarios.Add("desarrolladorti@humaya.com.mx");

                string[] para = destinatarios
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                await NEEnviarM365.Enviar(config, "GESTION JURIDICA DE " + datos_correo.nombre_cliente, body(datos_correo), para);
                return true;
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
            }

        }

        static string body(mdl_Clientes_Juridico_Correo datos_Correo)

        {

            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo.jpg");

            string logo64 = Convert.ToBase64String(logo);

            String sHtml;

            sHtml = "<HTML>\n" +
               "<HEAD>\n" +
               "<TITLE>GESTION CREDITO</TITLE>\n" +
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
                                                "GESTION DE CREDITO \n" +
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
                        " CLIENTE: " + datos_Correo.nombre_cliente +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " ESTATUS: " + "Enviado a " + datos_Correo.estatus +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " COMENTARIOS: " + datos_Correo.comentarios +
                        "</td>\n" +
                    "</tr>\n" +
                    //"<tr>\n" +
                    //    "<td style=\"padding:4px;border-bottom:1px solid #afb69d\" colspan=\"2\">\n" +
                    //       " COMENTARIO: " + datos_Correo.mdldatos.comentarios +
                    //    "</td>\n" +
                    //"</tr>\n" +
               "</tbody>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }
    }
}
