using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Notifications.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Analisis
{
    public class NotificacionComentarios
    {
        public static string _Mensaje { get; private set; }

        // Limpia vacios y repetidos antes de mandar a Microsoft Graph.
        private static string[] Destinatarios(IEnumerable<string?> correos)
        {
            return correos
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }
        //         public static void Enviar(string Correo, string _tipoSolicitud, string _folio, string _vendedor, string _cliente, string _linea, string
        //_monto)
        public static async Task<bool> Enviar(mdl_Correo_M365 config, mdlAnalisis_Email_View datos_correo)
        {
            try
            {
                var para = new List<string?>();
                //foreach (mdlCorreo_Notificacion notificacion in datos_correo.notificacion)
                //{
                //    objeto_mail.To.Add(new MailAddress(notificacion.correo));
                //}
                para.Add(datos_correo.detalle.correo_gerente_sucursal);
                para.Add(datos_correo.detalle.correo_vendedor);
                para.Add(datos_correo.detalle.correo_responsable_credito);
                //objeto_mail.To.Add(datos_correo.detalle.correo_responsable_credito2);
                //objeto_mail.To.Add(datos_correo.detalle.correo_responsable_credito3);

                string asunto = datos_correo.detalle.asunto + datos_correo.detalle.proceso;
                string cuerpo = body(datos_correo);
                await NEEnviarM365.Enviar(config, asunto, cuerpo, Destinatarios(para));
                return true;
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
            }

        }


        public static async Task<bool> Enviar_Mhusa(mdl_Correo_M365 config, mdlAnalisis_Mhusa datos_correo)
        {
            try
            {
                var para = new List<string?>();
                foreach (mdlSolicitudCredito_Enviar notificacion in datos_correo.mdlSolicitud)
                {
                    para.Add(notificacion.correo);
                }

                //objeto_mail.To.Add("desarrolladorti@humaya.com.mx");

                string asunto = datos_correo.mdldatos.asunto;
                string cuerpo = bodyMhusa(datos_correo);
                await NEEnviarM365.Enviar(config, asunto, cuerpo, Destinatarios(para));
                return true;
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
            }

        }
        public static async Task<bool> EnviarProcesoFinalizado(mdl_Correo_M365 config, IEnumerable <mdlCorreo_Notificacion> datos_correo, string folio)
        {
            try
            {
                var para = new List<string?>();
                foreach (mdlCorreo_Notificacion notificacion in datos_correo)
                {
                    para.Add(notificacion.correo);
                }
                string asunto = "SOLICITUD: " + folio;
                string cuerpo = body(folio);
                await NEEnviarM365.Enviar(config, asunto, cuerpo, Destinatarios(para));
                return true;
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
            }

        }

        public static async Task<bool> EnviarOperacionCondicionada(mdl_Correo_M365 config, mdlSC_Credito_Condicionado datos_correo)
        {
            try
            {
                var para = new List<string?>();
                foreach (mdlSolicitudCredito_Enviar notificacion in datos_correo.mdlSolicitud)
                {
                    para.Add(notificacion.correo);
                }

                //objeto_mail.To.Add("desarrolladorti@humaya.com.mx");

                string asunto = datos_correo.mdldatos.asunto;
                string cuerpo = bodyCondicionado(datos_correo);
                await NEEnviarM365.Enviar(config, asunto, cuerpo, Destinatarios(para));
                return true;
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
            }

        }

        public static async Task<bool> EnviarNotificacionOperacionCondicionada(mdl_Correo_M365 config, mdl_Notificacion_Correo_Solicitud_Condicionada_View datos_correo)
        {
            try
            {
                var para = new List<string?>();
                foreach (mdlSolicitudCredito_Enviar notificacion in datos_correo.mdlSolicitud)
                {
                    para.Add(notificacion.correo);
                }

                //objeto_mail.To.Add("desarrolladorti@humaya.com.mx");

                string asunto = datos_correo.mdldatos.asunto;
                string cuerpo = bodyNotificacionCondicionado(datos_correo);
                await NEEnviarM365.Enviar(config, asunto, cuerpo, Destinatarios(para));
                return true;
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
            }

        }

        static string bodyMhusa(mdlAnalisis_Mhusa datos_Correo)

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
                        " FOLIO: " + datos_Correo.mdldatos.folio +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " VENDEDOR: " + datos_Correo.mdldatos.asesor+
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " CLIENTE: " + datos_Correo.mdldatos.cliente +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\" colspan=\"2\">\n" +
                           " COMENTARIO: " + datos_Correo.mdldatos.comentarios +
                        "</td>\n" +
                    "</tr>\n" +
               "</tbody>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }


        public static async Task<bool> EnviarNotificacionFacturacion(mdl_Correo_M365 config, mdlAnalisis_Email_Facturacion datos_correo)
        {

            try
            {
                var para = new List<string?>();

                para.Add(datos_correo.correo_responsable_credito);
                if (datos_correo.correo_responsable_credito2 != null)
                {
                    para.Add(datos_correo.correo_responsable_credito2);
                }
                if (datos_correo.correo_responsable_credito3 != null)
                {
                    para.Add(datos_correo.correo_responsable_credito3);
                }
                para.Add(datos_correo.correo_gerente_sucursal);
                para.Add(datos_correo.correo_vendedor);
                if (datos_correo.correo_responsable_cajera != null)
                {
                    para.Add(datos_correo.correo_responsable_cajera);
                }

                //objeto_mail.To.Add(new MailAddress("desarrolladorti@humaya.com.mx"));
                //objeto_mail.To.Add(new MailAddress("desarrolladorti2@humaya.com.mx"));

                string asunto = datos_correo.asunto + datos_correo.proceso;
                string cuerpo = body(datos_correo);
                await NEEnviarM365.Enviar(config, asunto, cuerpo, Destinatarios(para));
                return true;
            }

            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
            }

        }

        public static async Task<bool> EnviarModificacionDocumentosAprobadosCondicionado(mdl_Correo_M365 config, mdl_Analisis_Documentacion_Aceptada_Condicionado_View datos_correo)
        {
            try
            {
                var para = new List<string?>();
                foreach (mdlSolicitudCredito_Enviar notificacion in datos_correo.mdlSolicitud)
                {
                    para.Add(notificacion.correo);
                }
                //objeto_mail.To.Add("desarrolladorti@humaya.com.mx");
                //objeto_mail.To.Add(datos_correo.detalle.correo_vendedor);
                //objeto_mail.To.Add(datos_correo.detalle.correo_responsable_credito);
                //objeto_mail.To.Add(datos_correo.detalle.correo_responsable_credito2);
                //objeto_mail.To.Add(datos_correo.detalle.correo_responsable_credito3);

                string asunto = datos_correo.mdldatos.asunto + ' ' + datos_correo.mdldatos.folio;
                string cuerpo = bodyAnalisisDocumentacionAceptadaCondicionado(datos_correo.mdldatos);
                await NEEnviarM365.Enviar(config, asunto, cuerpo, Destinatarios(para));
                return true;
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
            }

        }

        public static async Task<bool> EnviarCargaDocumentosAprobadosCondicionado(mdl_Correo_M365 config, mdl_Cargar_Documentacion_Aceptada_Condicionado_View datos_correo)
        {
            try
            {
                var para = new List<string?>();
                foreach (mdlSolicitudCredito_Enviar notificacion in datos_correo.mdlSolicitud)
                {
                    para.Add(notificacion.correo);
                }
                //objeto_mail.To.Add("desarrolladorti@humaya.com.mx");
                //objeto_mail.To.Add(datos_correo.detalle.correo_vendedor);
                //objeto_mail.To.Add(datos_correo.detalle.correo_responsable_credito);
                //objeto_mail.To.Add(datos_correo.detalle.correo_responsable_credito2);
                //objeto_mail.To.Add(datos_correo.detalle.correo_responsable_credito3);

                string asunto = datos_correo.mdldatos.asunto + ' ' + datos_correo.mdldatos.folio;
                string cuerpo = bodyAnalisisDocumentacionAceptadaCondicionado(datos_correo.mdldatos);
                await NEEnviarM365.Enviar(config, asunto, cuerpo, Destinatarios(para));
                return true;
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
            }

        }

        public static async Task<bool> EnviarCargaDocumentosVendedor(mdl_Correo_M365 config, mdlSolicitud_CRedito_Documentacion_Email datos_correo, string folio)
        {
            try
            {
                var para = new List<string?>();
                foreach (mdlSolicitudCredito_Enviar notificacion in datos_correo.mdlSolicitud)
                {
                    para.Add(notificacion.correo);
                }
                //objeto_mail.To.Add("desarrolladorti@humaya.com.mx");
                //objeto_mail.To.Add(datos_correo.detalle.correo_vendedor);
                //objeto_mail.To.Add(datos_correo.detalle.correo_responsable_credito);
                //objeto_mail.To.Add(datos_correo.detalle.correo_responsable_credito2);
                //objeto_mail.To.Add(datos_correo.detalle.correo_responsable_credito3);

                string asunto = "Carga de Documentos";
                string cuerpo = bodyAnalisisDocumentacionCargaVendedor(datos_correo.notificar, folio);
                await NEEnviarM365.Enviar(config, asunto, cuerpo, Destinatarios(para));
                return true;
            }
            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
            }

        }

        public static async Task<bool> EnviarAutorizarFacturacion(mdl_Correo_M365 config, mdlAnalisisAutorizacionFacturacion_Email datos_correo)
        {

            try
            {
                var para = new List<string?>();
                para.Add(datos_correo.correo_cajera);
                para.Add(datos_correo.correo_responsable_credito);
                para.Add(datos_correo.correo_gerente_sucursal);
                para.Add(datos_correo.correo_vendedor);

                //objeto_mail.To.Add(new MailAddress("desarrolladorti2@hunaya.com.mx"));
                //objeto_mail.To.Add(new MailAddress("desarrolladorti@humaya.com.mx"));


                string asunto = datos_correo.asunto + datos_correo.proceso;
                string cuerpo = body(datos_correo);
                await NEEnviarM365.Enviar(config, asunto, cuerpo, Destinatarios(para));
                return true;
            }

            catch (Exception ex)
            {
                _Mensaje = ex.Message;
                return false;
            }

        }

        static string body(mdlAnalisis_Email_Facturacion datos_Correo)

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
                    "margin-top:40px;\n" + 
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
                                                "SOLICITUD DE CREDITO \n" +
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
                           "<div style=\"font-size:18px;\">" + datos_Correo.proceso + " " + datos_Correo.estatus + "</div>\n" +
                        "</th>\n" +
                    "</tr>\n" +
                "</thead>\n" +
               "<tbody>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;text-align:left;margin-left:10px\">\n" +
                            datos_Correo.comentarios +
                        "</td>\n" +
                    "</tr>\n" +
               "</tbody>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }

        static string body(string folio)

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
                    "margin-top:40px;\n" + 
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
                                                "SOLICITUD DE CREDITO \n" +
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
                           "<div style=\"font-size:18px;\">" + "PROCESO DE SOLICITUD: " + folio + "</div>\n" +
                        "</th>\n" +
                    "</tr>\n" +
                "</thead>\n" +
               "<tbody>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;text-align:left;margin-left:10px\">\n" +
                            "FINALIZADA" +
                        "</td>\n" +
                    "</tr>\n" +
               "</tbody>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }

        static string body(mdlAnalisis_Email_View datos_Correo)

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
                                                "SOLICITUD DE CREDITO \n" +
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
                           "<div style=\"font-size:18px;\">" + datos_Correo.detalle.proceso + " " + datos_Correo.detalle.estatus + "</div>\n" +
                        "</th>\n" +
                    "</tr>\n" +
                "</thead>\n" +
               "<tbody>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;text-align:left;margin-left:10px\">\n" +
                            datos_Correo.detalle.comentarios +
                        "</td>\n" +
                    "</tr>\n" +
               "</tbody>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }

        static string body(mdlAnalisisAutorizacionFacturacion_Email datos_Correo)

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
                                                "SOLICITUD DE CREDITO \n" +
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
                           "<div style=\"font-size:18px;\">" + datos_Correo.proceso + " " + datos_Correo.estatus + "</div>\n" +
                        "</th>\n" +
                    "</tr>\n" +
                "</thead>\n" +
               "<tbody>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;text-align:left;margin-left:10px\">\n" +
                            datos_Correo.comentarios +
                        "</td>\n" +
                    "</tr>\n" +
               "</tbody>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }

        static string bodyCondicionado(mdlSC_Credito_Condicionado datos_Correo)

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
                        " FOLIO: " + datos_Correo.mdldatos.folio +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " VENDEDOR: " + datos_Correo.mdldatos.asesor +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " CLIENTE: " + datos_Correo.mdldatos.cliente +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\" colspan=\"2\">\n" +
                           " COMENTARIO: " + datos_Correo.mdldatos.comentarios +
                        "</td>\n" +
                    "</tr>\n" +
               "</tbody>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }

        static string bodyNotificacionCondicionado(mdl_Notificacion_Correo_Solicitud_Condicionada_View datos_Correo)

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
                        " FOLIO: " + datos_Correo.mdldatos.folio +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " VENDEDOR: " + datos_Correo.mdldatos.asesor +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " CLIENTE: " + datos_Correo.mdldatos.cliente +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\" colspan=\"2\">\n" +
                           " COMENTARIO: " + datos_Correo.mdldatos.comentarios +
                        "</td>\n" +
                    "</tr>\n" +
               "</tbody>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }

        static string bodyAnalisisDocumentacionAceptadaCondicionado(mdldatos_notificacion datos_Correo)

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
                        " VENDEDOR: " + datos_Correo.asesor +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " CLIENTE: " + datos_Correo.cliente +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\" colspan=\"2\">\n" +
                           " COMENTARIO: " + datos_Correo.comentarios +
                        "</td>\n" +
                    "</tr>\n" +
               "</tbody>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }

        static string bodyAnalisisDocumentacionCargaVendedor(mdl_Notificar datos_Correo, string folio)

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
                        " FOLIO: " + folio +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " VENDEDOR: " + datos_Correo.vendedor +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\"  colspan=\"2\">\n" +
                        " CLIENTE: " + datos_Correo.cliente +
                        "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                        "<td style=\"padding:4px;border-bottom:1px solid #afb69d\" colspan=\"2\">\n" +
                           " Carga de documentación del vendedor completa " +
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
