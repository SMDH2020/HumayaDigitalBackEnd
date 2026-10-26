using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HD.Notifications
{
    public class GenerarCorreo
    {
        // ---------------------------------------------------------------
        // JSON esperado:
        // {
        //   "folio": "ACT202610145",                   (opcional)
        //   "seccion": "Seguimiento de actividades",
        //   "titulo": "Nuevo ticket de soporte",
        //   "accion": "Ticket creado",                 (opcional)
        //   "cuerpo": {                                (nombre: contenido)
        //       "Actividad": "Error al abrir aplicación",
        //       "Sala": "SOPORTE TI",
        //       "Reportado por": "Guadalupe Olivas Leon"
        //   },
        //   "comentario": "prueba",                    (opcional)
        //   "instrucciones": "Ingresa al sistema..."   (opcional)
        // }
        // Los nombres de las propiedades no distinguen mayúsculas/minúsculas.
        // ---------------------------------------------------------------
        public static string RenderizarDesdeJson(string json)
            => Renderizar(DesdeJson(json));

        public static mdl_Datos_Correo DesdeJson(string json)
        {
            using var doc = JsonDocument.Parse(json);
            var raiz = doc.RootElement;
            var datos = new mdl_Datos_Correo();

            foreach (var p in raiz.EnumerateObject())
            {
                switch (p.Name.ToLowerInvariant())
                {
                    case "folio": datos.Folio = Texto(p.Value); break;
                    case "seccion": datos.Seccion = Texto(p.Value) ?? ""; break;
                    case "titulo": datos.Titulo = Texto(p.Value) ?? ""; break;
                    case "accion": datos.Accion = Texto(p.Value); break;
                    case "comentario": datos.Comentario = Texto(p.Value); break;
                    case "instrucciones": datos.Instrucciones = Texto(p.Value); break;
                    case "cuerpo":
                        if (p.Value.ValueKind == JsonValueKind.Object)
                        {
                            // JsonDocument conserva el orden original del JSON.
                            foreach (var campo in p.Value.EnumerateObject())
                                datos.Cuerpo.Add(new(campo.Name, Texto(campo.Value)));
                        }
                        break;
                }
            }
            return datos;
        }

        private static string? Texto(JsonElement e) => e.ValueKind switch
        {
            JsonValueKind.Null or JsonValueKind.Undefined => null,
            JsonValueKind.String => e.GetString(),
            _ => e.GetRawText()   // números, booleanos, etc.
        };

        // Escapa HTML (evita que un valor rompa el correo o inyecte markup)
        // y respeta los saltos de línea.
        private static string Enc(string? s) =>
            WebUtility.HtmlEncode(s ?? "").Replace("\r\n", "\n").Replace("\n", "<br/>");

        public static string Renderizar(mdl_Datos_Correo d)
        {
            const string verde = "#2b5b25";

            // El logo siempre va incrustado: hay que adjuntarlo con Content-ID "logoHumaya".
            const string marca =
                "<img src=\"cid:logoHumaya\" alt=\"Humaya Digital\" height=\"40\" style=\"height:40px;max-height:40px;width:auto;display:block;border:0;\" />";

            string folioHtml = !string.IsNullOrWhiteSpace(d.Folio)
                ? "<span style=\"display:inline-block;background-color:#1e4a19;color:#ffffff;font-size:12px;" +
                  "font-family:Consolas,Menlo,monospace;padding:5px 12px;border-radius:20px;letter-spacing:.4px;\">" +
                  "FOLIO " + Enc(d.Folio) + "</span>"
                : "";

            // Todo se arma con <table> y bgcolor: Outlook de escritorio (motor de Word)
            // ignora el fondo/padding de los <div> y dejaba franjas blancas entre bloques.
            var sb = new StringBuilder();

            sb.Append("<html><body style=\"margin:0;padding:0;background-color:#f7f8f6;font-family:Segoe UI,Roboto,Helvetica,Arial,sans-serif;\">");
            sb.Append("<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" bgcolor=\"#f7f8f6\" style=\"background-color:#f7f8f6;\"><tr><td align=\"center\" style=\"padding:16px;\">");
            sb.Append("<table role=\"presentation\" width=\"600\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:100%;max-width:600px;border-collapse:collapse;\">");

            // Encabezado: marca + folio
            sb.Append("<tr><td bgcolor=\"" + verde + "\" style=\"background-color:" + verde + ";background-image:linear-gradient(135deg,#255220,#367c2b);padding:14px 22px;\">");
            sb.Append("<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr>");
            sb.Append("<td style=\"vertical-align:middle;\">" + marca + "</td>");
            sb.Append("<td align=\"right\" style=\"vertical-align:middle;text-align:right;\">" + folioHtml + "</td>");
            sb.Append("</tr></table></td></tr>");

            // Tarjeta blanca (cada bloque es una fila con bgcolor blanco)
            const string blanco = " bgcolor=\"#ffffff\" style=\"background-color:#ffffff;";

            // Sección + título + acción
            sb.Append("<tr><td" + blanco + "padding:18px 22px 2px 22px;\">");
            if (!string.IsNullOrWhiteSpace(d.Seccion))
                sb.Append("<div style=\"font-size:11px;letter-spacing:1px;color:#8d9688;text-transform:uppercase;font-weight:600;\">" + Enc(d.Seccion) + "</div>");
            sb.Append("<h2 style=\"margin:4px 0 10px 0;color:#1f2937;font-size:20px;\">" + Enc(d.Titulo) + "</h2>");
            if (!string.IsNullOrWhiteSpace(d.Accion))
            {
                var e = d.EstiloAccion ?? EstiloAccion.Neutro;
                sb.Append("<span style=\"display:inline-block;background-color:" + e.Fondo + ";color:" + e.Texto +
                          ";border:1px solid " + e.Borde + ";font-size:12px;font-weight:700;letter-spacing:.4px;padding:6px 14px;border-radius:20px;\">" +
                          Enc(d.Accion) + "</span>");
            }
            sb.Append("</td></tr>");

            // Cuerpo (nombre: contenido)
            var filas = d.Cuerpo.Where(c => !string.IsNullOrWhiteSpace(c.Value)).ToList();
            if (filas.Count > 0)
            {
                sb.Append("<tr><td" + blanco + "padding:10px 22px 2px 22px;\">");
                sb.Append("<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" bgcolor=\"#f7f8f6\" style=\"background-color:#f7f8f6;border:1px solid #eef0ec;border-collapse:separate;border-radius:10px;\"><tr><td style=\"padding:12px 16px 2px 16px;\">");
                foreach (var (nombre, valor) in filas)
                {
                    sb.Append("<div style=\"margin-bottom:8px;\">");
                    sb.Append("<div style=\"font-size:11px;color:#8d9688;text-transform:uppercase;letter-spacing:.5px;font-weight:600;\">" + Enc(nombre) + "</div>");
                    sb.Append("<div style=\"color:#1f2937;font-size:14px;margin-top:3px;line-height:1.4;\">" + Enc(valor) + "</div>");
                    sb.Append("</div>");
                }
                sb.Append("</td></tr></table></td></tr>");
            }

            // Comentario (opcional)
            if (!string.IsNullOrWhiteSpace(d.Comentario))
            {
                sb.Append("<tr><td" + blanco + "padding:8px 22px 2px 22px;\">");
                sb.Append("<div style=\"font-size:11px;color:#8d9688;text-transform:uppercase;letter-spacing:.5px;font-weight:600;margin-bottom:4px;\">Comentario</div>");
                // La barra amarilla es el border-left de la celda: una sola pieza, no se corta
                // (antes era una celda aparte de 3px y Outlook la partía).
                sb.Append("<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr>");
                sb.Append("<td bgcolor=\"#fffdf2\" style=\"background-color:#fffdf2;border-left:3px solid #fedb05;padding:10px 14px;color:#4f5a4b;font-size:14px;line-height:1.5;\">" + Enc(d.Comentario) + "</td>");
                sb.Append("</tr></table></td></tr>");
            }

            // Instrucciones (opcional)
            if (!string.IsNullOrWhiteSpace(d.Instrucciones))
            {
                sb.Append("<tr><td" + blanco + "padding:12px 22px 16px 22px;color:#4f5a4b;font-size:13px;line-height:1.5;\">" +
                          Enc(d.Instrucciones) + "</td></tr>");
            }
            else
            {
                // Espaciado inferior para que la tarjeta no quede pegada al pie.
                sb.Append("<tr><td" + blanco + "height:10px;font-size:0;line-height:0;\">&nbsp;</td></tr>");
            }

            // Pie
            sb.Append("<tr><td bgcolor=\"#1f2937\" align=\"center\" style=\"background-color:#1f2937;padding:10px 22px;text-align:center;\">" +
                      "<span style=\"color:#d5d9d0;font-size:11px;letter-spacing:.3px;\">Correo autom&aacute;tico &middot; Humaya Digital</span></td></tr>");

            sb.Append("</table></td></tr></table></body></html>");
            return sb.ToString();
        }
    }
}
