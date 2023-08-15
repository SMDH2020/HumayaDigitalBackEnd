using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HDNotificacionesEmail.Auth
{
    public static class NECodigoSeguridad
    {
        public static async Task<string> enviar(string _para, string _codigo)
        {
            List<string> para = new List<string>() { _para };
            string bodyhtml = body(_codigo);
            await NEEnviar.Click("Codigo de Autenticación", "humayadigital@humaya.com.mx", "Xs63hi3@5", bodyhtml, para.ToArray());
            return "Correo enviado con exito";
        }
        static string body(string _codigo)
        {
            byte[] logo = File.ReadAllBytes("C:\\SMDH\\logo.png");
            string logo64 = Convert.ToBase64String(logo);
            String sHtml;
            sHtml = "<HTML>\n" +
               "<HEAD>\n" +
               "<TITLE>HUMAYA DIGITAL</TITLE>\n" +
               "</HEAD>\n" +
               "<BODY style=\"text-align:center;\"><P>\n" +
               "<img width='200' height='80' src='data:image/png;base64," + logo64 + "'\n" +
               "<h1 style=\"font-size:16;\"><Font Color='#235B34'>CODIGO DE AUTENTICACION: </Font></h1></P>\n" +
               "<table cellPadding=\"0\" cellSpacing =\"0\">\n" +
               " <tr>\n" +
               "   <td width='120'>Codigo:\n" +
               "   </td>\n" +
               "   <td>" + _codigo + "\n" +
               "   </td>\n" +
               " </tr>\n" +
               "</table>\n" +
               "</BODY>\n" +
               "</HTML>";
            return sHtml;
        }
    }
}
