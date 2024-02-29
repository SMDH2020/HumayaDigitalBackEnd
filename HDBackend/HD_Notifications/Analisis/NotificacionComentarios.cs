using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Analisis
{
    public class NotificacionComentarios
    {
        public static string _Mensaje { get; private set; }
        //         public static void Enviar(string Correo, string _tipoSolicitud, string _folio, string _vendedor, string _cliente, string _linea, string
        //_monto)
        public static Task<bool> Enviar(mdlAnalisis_Email datos_correo)
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

                objeto_mail.To.Add(new MailAddress(datos_correo.correo_responsable_credito));
                objeto_mail.To.Add(new MailAddress(datos_correo.correo_gerente_sucursal));
                objeto_mail.To.Add(new MailAddress(datos_correo.correo_vendedor));

                objeto_mail.Subject = datos_correo.asunto + datos_correo.proceso;
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
        public static Task<bool> Enviar(mdlAnalisis_Email datos_correo)
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

                objeto_mail.To.Add(new MailAddress(datos_correo.correo_responsable_credito));
                objeto_mail.To.Add(new MailAddress(datos_correo.correo_gerente_sucursal));
                objeto_mail.To.Add(new MailAddress(datos_correo.correo_vendedor));

                objeto_mail.Subject = datos_correo.asunto + datos_correo.proceso;
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

        static string body(mdlAnalisis_Email datos_Correo)

        {

            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo-reports.jpg");

            string logo64 = Convert.ToBase64String(logo);

            String sHtml;

            sHtml = "<HTML>\n" +
               "<HEAD>\n" +
               "<TITLE>SOLICITUD DE CREDITO</TITLE>\n" +
               "</HEAD>\n" +
               "<BODY style=\"text-align:center;\"><P>\n" +
                "<div style=\"\">\n" +
                    "<img height='100' src='data:image/png;base64," + logo64 + "'/>\n" +
                "</div>\n" +
                "<h1  style =\"\"> SOLICITUD DE CREDITO </h1>\n" +
                "</div>\n" +
               "<h1 style=\"font-size:16; text-transform: uppercase\"><Font Color='#235B34'>" + datos_Correo.proceso + "</Font></h1></P>\n" +
               "<h1 style=\"font-size:16; text-transform: uppercase\"><Font Color='#235B34'>" + datos_Correo.estatus + "</Font></h1></P>\n" +
              "<table style=\"border: 1px solid green;border-radius:5px; padding: 10px; spacing:0; display: flex; align - items:center; justify - content:center; margin - left:80px; margin - right:80px; '\">\n" +
              "<tr>\n"+
                "<td>" + datos_Correo.comentarios +"222</td>\n" +
            " </tr>\n" +
            "</table>\n" +
            "</BODY>\n" +
            "</HTML>";

            return sHtml;

        }

    }
}
