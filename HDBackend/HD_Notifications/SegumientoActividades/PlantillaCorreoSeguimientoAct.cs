namespace HD.Notifications.SeguimientoActividades
{
    // Datos necesarios para armar el cuerpo del correo. Se separa del
    // modelo mdlSeguimiento_Email (que es lo que llega del controller/SP)
    // para que la plantilla no dependa de esa forma en particular.
    public class DatosCorreoSeguimientoAct
    {
        public string TituloPrincipal { get; set; } = "";
        public string? Folio { get; set; }
        public string? Actividad { get; set; }
        public string? Sala { get; set; }
        public string? Comentario { get; set; }
        public string? NombrePersona { get; set; }
        public string? SubtituloPersona { get; set; }
        public string? Estatus { get; set; }
    }

    // Plantilla HTML compartida por las notificaciones de Seguimiento de
    // Actividades (ticket creado, cambio de estatus, comentario nuevo) --
    // antes cada clase traía su propio HTML casi idéntico. Los colores
    // siguen la misma paleta "John Deere" que ya usa el resto del sistema
    // (Styles/PermisosUsuarios/ConfigurarLeads/_ConfigurarLeadsScreen.scss)
    // y el pill de estatus usa exactamente los mismos tonos que
    // .status-badge--estatus-* en Styles/SeguimientoActividades/_SA_GestionModales.scss,
    // para que el correo se sienta parte de la misma aplicación.
    public static class PlantillaCorreoSeguimientoAct
    {
        public static (string etiqueta, string fondo, string color, string borde) EstiloEstatus(string? estatus)
        {
            switch (estatus)
            {
                case "C": return ("TICKET CREADO", "#eef0ec", "#1f2937", "#b8bfb3");
                case "P": return ("EN PROCESO", "#fff8de", "#8a6d00", "#fedb05");
                case "F": return ("FINALIZADO", "#e8f0fb", "#1f3a5f", "#3c587c");
                case "A": return ("ACEPTADO / AUTORIZADO", "#edf7eb", "#255220", "#97c459");
                case "R": return ("RECHAZADO", "#fcebeb", "#791f1f", "#b3261e");
                case "M": return ("NUEVO COMENTARIO", "#f2eefc", "#4b2e83", "#8e44ad");
                default: return (string.IsNullOrWhiteSpace(estatus) ? "ACTUALIZACIÓN" : estatus!, "#eef0ec", "#1f2937", "#b8bfb3");
            }
        }

        // incluirLogo: si el archivo del logo no se pudo leer en el
        // servidor, se manda el correo sin la imagen (con una marca de
        // texto) en vez de mostrar un ícono roto.
        public static string Renderizar(DatosCorreoSeguimientoAct datos, bool incluirLogo)
        {
            var (etiqueta, fondo, color, borde) = EstiloEstatus(datos.Estatus);

            // El tamaño va también como atributos HTML (width/height), no
            // solo en "style": varios clientes de correo (Outlook/Exchange
            // entre ellos) recortan el style del <img> por seguridad y lo
            // muestran a su tamaño real si solo se limita por CSS -- con
            // el atributo "height" y sin "width" el navegador/cliente
            // escala manteniendo la proporción original del logo.
            string marca = incluirLogo
                ? "<img src=\"cid:logoHumaya\" alt=\"Humaya Digital\" height=\"40\" style=\"height:40px;max-height:40px;width:auto;display:block;border:0;\" />"
                : "<span style=\"font-size:17px;font-weight:700;letter-spacing:.4px;color:#ffffff;\">HUMAYA&nbsp;DIGITAL</span>";

            // Color sólido en vez de rgba() -- Outlook de escritorio (motor
            // de Word) no interpreta transparencias y lo pinta negro.
            string folioHtml = !string.IsNullOrWhiteSpace(datos.Folio)
                ? "<span style=\"display:inline-block;background-color:#1e4a19;color:#ffffff;font-size:12px;" +
                  "font-family:Consolas,Menlo,monospace;padding:5px 12px;border-radius:20px;letter-spacing:.4px;\">" +
                  "FOLIO " + datos.Folio + "</span>"
                : "";

            string filaDato(string etiquetaCampo, string? valor)
            {
                if (string.IsNullOrWhiteSpace(valor)) return "";
                return
                    "<div style=\"margin-bottom:12px;\">" +
                        "<div style=\"font-size:11px;color:#8d9688;text-transform:uppercase;letter-spacing:.5px;font-weight:600;\">" + etiquetaCampo + "</div>" +
                        "<div style=\"color:#1f2937;font-size:14px;margin-top:3px;line-height:1.4;\">" + valor + "</div>" +
                    "</div>";
            }

            string comentarioHtml = !string.IsNullOrWhiteSpace(datos.Comentario)
                ? "<div style=\"padding:4px 26px 4px 26px;\">" +
                    "<div style=\"font-size:11px;color:#8d9688;text-transform:uppercase;letter-spacing:.5px;font-weight:600;margin-bottom:6px;\">Comentario</div>" +
                    "<div style=\"background:#fffdf2;border-left:3px solid #fedb05;border-radius:6px;padding:12px 14px;color:#4f5a4b;font-size:14px;line-height:1.5;\">" +
                        datos.Comentario +
                    "</div>" +
                  "</div>"
                : "";

            string html =
            "<html><body style=\"margin:0;padding:0;background-color:#f7f8f6;font-family:Segoe UI,Roboto,Helvetica,Arial,sans-serif;\">" +

                "<div style=\"width:100%;padding:32px 16px;\">" +
                    "<table role=\"presentation\" align=\"center\" width=\"100%\" style=\"max-width:600px;margin:0 auto;border-collapse:collapse;\"><tr><td>" +

                        // Header verde (marca + folio). El degradado se manda
                        // como "background-image" además del color sólido en
                        // "bgcolor"/"background-color": los navegadores y
                        // clientes modernos usan el degradado, y Outlook de
                        // escritorio (que no entiende linear-gradient) cae de
                        // vuelta al verde sólido en vez de quedar en blanco.
                        "<table role=\"presentation\" width=\"100%\" style=\"border-collapse:collapse;\"><tr>" +
                            "<td bgcolor=\"#2b5b25\" style=\"background-color:#2b5b25;background-image:linear-gradient(135deg,#255220,#367c2b);border-radius:14px 14px 0 0;padding:20px 26px;\">" +
                                "<table role=\"presentation\" width=\"100%\"><tr>" +
                                    "<td style=\"vertical-align:middle;\">" + marca + "</td>" +
                                    "<td style=\"vertical-align:middle;text-align:right;\">" + folioHtml + "</td>" +
                                "</tr></table>" +
                            "</td>" +
                        "</tr></table>" +

                        // Tarjeta blanca
                        "<div style=\"background:#ffffff;border-radius:0 0 14px 14px;box-shadow:0 6px 24px rgba(31,41,55,0.08);overflow:hidden;\">" +

                            "<div style=\"padding:26px 26px 6px 26px;\">" +
                                "<div style=\"font-size:11px;letter-spacing:1px;color:#8d9688;text-transform:uppercase;font-weight:600;\">Seguimiento de actividades</div>" +
                                "<h2 style=\"margin:6px 0 14px 0;color:#1f2937;font-size:20px;\">" + datos.TituloPrincipal + "</h2>" +
                                "<span style=\"display:inline-block;background:" + fondo + ";color:" + color + ";border:1px solid " + borde + ";" +
                                    "font-size:12px;font-weight:700;letter-spacing:.4px;padding:6px 14px;border-radius:20px;\">" + etiqueta + "</span>" +
                            "</div>" +

                            "<div style=\"padding:18px 26px 4px 26px;\">" +
                                "<div style=\"background:#f7f8f6;border:1px solid #eef0ec;border-radius:10px;padding:16px 18px;\">" +
                                    filaDato("Actividad", datos.Actividad) +
                                    filaDato("Sala", datos.Sala) +
                                    filaDato(datos.SubtituloPersona ?? "Usuario", datos.NombrePersona) +
                                "</div>" +
                            "</div>" +

                            comentarioHtml +

                            "<div style=\"padding:20px 26px 26px 26px;color:#4f5a4b;font-size:13px;line-height:1.5;\">" +
                                "Ingresa al sistema de Seguimiento de Actividades para ver el detalle completo de este ticket." +
                            "</div>" +

                            "<div style=\"background:#1f2937;padding:14px 26px;text-align:center;\">" +
                                "<span style=\"color:#d5d9d0;font-size:11px;letter-spacing:.3px;\">Correo automático &middot; Humaya Digital</span>" +
                            "</div>" +

                        "</div>" +

                    "</td></tr></table>" +
                "</div>" +

            "</body></html>";

            return html;
        }
    }
}
