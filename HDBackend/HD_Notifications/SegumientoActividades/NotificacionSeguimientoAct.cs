using Dapper;
using System.Data.SqlClient;
using System.Net.Mail;

namespace HD.Notifications.SeguimientoActividades
{
    public class NotificacionSeguimientoAct
    {
        public static string _Mensaje { get; private set; }

        public static Task<bool> Enviar(mdlSeguimiento_Email datos, string conexion)
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


                var correos = ObtenerCorreosResponsables(datos.idSala, conexion);

                Console.WriteLine("ID SALA: " + datos.idSala);
                Console.WriteLine("TOTAL CORREOS: " + (correos?.Count ?? 0));

                if (correos == null || correos.Count == 0)
                {
                    Console.WriteLine("⚠ No hay responsables para esta sala");
                    return Task.FromResult(false);
                }
                foreach (var correo in correos)
                {
                    Console.WriteLine("CORREO: " + correo);

                    if (!string.IsNullOrEmpty(correo))
                        objeto_mail.To.Add(new MailAddress(correo));
                }
                /*objeto_mail.To.Add(new MailAddress("L21170252@culiacan.tecnm.mx"));*/
                /*objeto_mail.To.Add(new MailAddress("desarrolladorti3@humaya.com.mx"));*/

                objeto_mail.Subject = "Seguimiento de Actividad";
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = body(datos);

                client.EnableSsl = false;
                client.Send(objeto_mail);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR CORREO: " + ex.Message);
                throw; 
            }
        }

        private static List<string> ObtenerCorreosResponsables(int idSala, string conexion)
        {
            using (var con = new SqlConnection(conexion))
            {
                string query = @"
                    SELECT e.Correo
                    FROM Seguimiento_Actividades.dbo.Rel_Sala_Responsable r
                    INNER JOIN AppMH.dbo.Empleados e
                        ON r.IDEmpleado = e.IDEmpleado
                    WHERE r.idSala = @idSala
                    AND r.estado = 1
                    AND e.Estatus = 1
                ";

                return con.Query<string>(query, new { idSala }).ToList();
            }
        }

        static string body(mdlSeguimiento_Email datos)
        {
            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo.jpg");
            string logo64 = Convert.ToBase64String(logo);

            string estatusTexto = "";
            string colorEstatus = "";

            switch (datos.estatus)
            {
                case "C":
                    estatusTexto = "ACTIVIDAD CREADA";
                    colorEstatus = "#3498db";
                    break;
                case "P":
                    estatusTexto = "ACTIVIDAD EN PROCESO";
                    colorEstatus = "#f39c12";
                    break;
                case "F":
                    estatusTexto = "ACTIVIDAD FINALIZADA";
                    colorEstatus = "#2ecc71";
                    break;
                case "A":
                    estatusTexto = "ACTIVIDAD ACEPTADA";
                    colorEstatus = "#27ae60";
                    break;
                case "R":
                    estatusTexto = "ACTIVIDAD RECHAZADA";
                    colorEstatus = "#e74c3c";
                    break;
                case "M":
                    estatusTexto = "COMENTARIO";
                    colorEstatus = "#8e44ad";
                    break;
                default:
                    estatusTexto = datos.estatus;
                    colorEstatus = "#7f8c8d";
                    break;
            }

            string sHtml = "<html>" +
            "<body style='margin:0;padding:0;background-color:#f4f6f7;font-family:Arial;'>"

            + "<div style='width:100%;padding:30px 0;display:flex;justify-content:center;'>"

                + "<div style='width:600px;background:#ffffff;border-radius:10px;box-shadow:0 4px 10px rgba(0,0,0,0.1);overflow:hidden;'>"

                    + "<div style='background-color:#275027;color:#fff;padding:20px;text-align:center;'>"
                    + "<h2 style='margin:0;'>SEGUIMIENTO DE ACTIVIDAD</h2>"
                    + "</div>"


                    + "<div style='background:" + colorEstatus + ";color:#fff;text-align:center;padding:12px;font-size:18px;font-weight:bold;'>"
                        + estatusTexto +
                    "</div>"

                    + "<div style='padding:20px;text-align:left;'>"

                        + "<p style='margin:10px 0;'><b>Actividad:</b><br/>" + datos.actividad + "</p>"

                        + "<p style='margin:10px 0;'><b>Comentario:</b><br/>" + datos.comentarios + "</p>"

                        + "<p style='margin:10px 0;'><b>Usuario:</b><br/>" + datos.usuario + "</p>"

                    + "</div>"

                    + "<div style='background:#ecf0f1;padding:15px;text-align:center;font-size:13px;color:#555;'>"
                        + "Favor de ingresar al sistema para revisar el seguimiento."
                    + "</div>"

                + "</div>"

            + "</div>"

            + "</body></html>";

            return sHtml;
        }
    }
}