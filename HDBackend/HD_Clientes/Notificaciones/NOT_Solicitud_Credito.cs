using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Notificaciones
{
    public class NOT_Solicitud_Credito
    {
        public static string _Mensaje { get; private set; }
        public static void Enviar(string Correo, string _tipoSolicitud, string _folio, string _vendedor, string _cliente, string _linea, string _monto)

        {

            try

            {

                string password = "RC_2023?MH*";

                MailMessage objeto_mail = new MailMessage();

                SmtpClient client = new SmtpClient();

                client.Port = 587;

                client.Host = "correo.humaya.com.mx";

                client.Timeout = 10000;

                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.UseDefaultCredentials = false;

                client.Credentials = new System.Net.NetworkCredential("relojchecador@humaya.com.mx", password);

                objeto_mail.From = new MailAddress("relojchecador@humaya.com.mx");

                objeto_mail.To.Add(new MailAddress(Correo));

                objeto_mail.Subject = "Nueva solicitud de credito";

                objeto_mail.IsBodyHtml = true;

                objeto_mail.Body = body( _tipoSolicitud,  _folio,  _vendedor,  _cliente,  _linea,  _monto);

                client.EnableSsl = false;

                client.Send(objeto_mail);

            }

            catch (Exception ex)

            {

                _Mensaje = ex.Message;

            }

        }

        static string body(string _tipoSolicitud, string _folio, string _vendedor, string _cliente, string _linea, string _monto )

        {

            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo.png");

            string logo64 = Convert.ToBase64String(logo);

            String sHtml;

            sHtml = "<HTML>\n" +

               "<HEAD>\n" +

               "<TITLE>SOLICITUD DE CREDITO</TITLE>\n" +

               "</HEAD>\n" +

               "<BODY style=\"text-align:center;\"><P>\n" +
                "<div style=\"position:absolute;top:0px;left:60px\">\n" +
                    "<img width='150' src='data:image/png;base64," + logo64 + "'/>\n" +
                "</div>\n" +
                "<div style=\"background-color: #477c2c; height: 57px;margin-top:51px;;text-align:center;display:flex;align-items:center;justify-content:center;margin-bottom:60px\">\n" +
                "<h1 style=\"font-size:20px;margin-left:40pxpx;color:#fff\"> SOLICITUD DE CREDITO </h1>\n" +
                "</div>\n" +


               "<h1 style=\"font-size:16;\"><Font Color='#235B34'>" + _tipoSolicitud + "</Font></h1></P>\n" +

              "<table style=\"border: 1px solid green;border-radius:5px; padding: 10px; spacing: 0;display:flex;align-items:center;justify-content:center;margin-left:80px;margin-right:80px;'\">\n" +

               " <tr\">\n" +

               "   <td style=\"text-align:center; width:50%;\">FOLIO:\n" +

               "   </td>\n" +

               "   <td style=\"text-align: left; width:50%;\">" + _folio + "\n" +

               "   </td>\n " +

               " </tr>\n" +

               " <tr>\n" +

               "   <td style=\"text-align:center; width:50%;\">VENDEDOR:\n" +

               "   </td>\n" +

               "   <td style=\"text-align: left; width:50%\">" + _vendedor + "\n" +

               "   </td>\n" +

               " </tr>\n" +

               " <tr>\n" +

               "   <td width= 200px>CLIENTE:\n" +

               "   </td>\n" +

               "   <td style=\"text-align: left;\">" + _cliente + "\n" +

               "   </td>\n" +

               " </tr>\n" +

               " <tr>\n" +

               "   <td width= 200px>LINEA:\n" +

               "   </td>\n" +

               "   <td style=\"text-align: left;\">" + _linea + "\n" +

               "   </td>\n" +

               " </tr>\n" +

               " <tr>\n" +

               "   <td width= 200px>MONTO:\n" +

               "   </td>\n" +

               "   <td style=\"text-align: left;\">" + _monto + "\n" +

               "   </td>\n" +

               " </tr>\n" +

               " <tr>\n" +

               "   <td colspan='2' height=30>" +

               "   </td>\n" +

               " </tr>\n" +

               " <tr>\n" +

               "   <td colspan='2'> <Font>El presente correo es de carácter informativo. En caso de que tenga justificación, favor de realizar el proceso correspondiente ante nómina.</Font> \n" +

               "   </td>\n" +

               " </tr>\n" +

               "</table>\n" +

               "</BODY>\n" +

               "</HTML>";

            return sHtml;

        }


    }
}
