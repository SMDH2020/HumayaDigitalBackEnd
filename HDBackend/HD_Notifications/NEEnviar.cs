﻿using HD.AccesoDatos;
using System.Net.Mail;

namespace HD.Notifications
{
    public class NEEnviar
    {
        public static Task<string> Click(string _asunto, string _correo, string _password, string _body, string[] para)
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
