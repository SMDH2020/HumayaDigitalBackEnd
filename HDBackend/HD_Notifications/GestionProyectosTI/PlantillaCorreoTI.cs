namespace HD.Notifications.GestionProyectosTI
{
    // Datos necesarios para armar el cuerpo del correo de Gestion de
    // Proyectos de TI. Un solo modelo/plantilla para los 5 eventos que
    // notifica el modulo (solicitud creada, alcance aprobado, funcionalidad
    // priorizada, funcionalidad enviada a revision, revision resuelta) --
    // solo cambia la etiqueta de estatus y las filas de datos.
    public class DatosCorreoTI
    {
        public string TituloPrincipal { get; set; } = "";
        public string? Folio { get; set; }
        public string? Etiqueta { get; set; }
        public string EtiquetaColor { get; set; } = "#3c587c";
        public string EtiquetaFondo { get; set; } = "#e8f0fb";
        public string EtiquetaBorde { get; set; } = "#3c587c";
        public string? NombreSolicitud { get; set; }
        public string? NombreFuncionalidad { get; set; }
        public string? NombrePersona { get; set; }
        public string? SubtituloPersona { get; set; }
        public string? MesAnio { get; set; }
        public string? Comentario { get; set; }
        public string? TextoFooter { get; set; }
    }

    // Plantilla HTML compartida por las notificaciones de Gestion de
    // Proyectos de TI. Mismo diseño "Humaya Digital" (verde, logo en linea,
    // pill de estatus, tarjeta blanca) ya usado en
    // HD_Notifications/SegumientoActividades/PlantillaCorreoSeguimientoAct.cs,
    // para que el correo se sienta parte de la misma aplicacion.
    public static class PlantillaCorreoTI
    {
        public static string Renderizar(DatosCorreoTI datos, bool incluirLogo)
        {
            string marca = incluirLogo
                ? "<img src=\"cid:logoHumaya\" alt=\"Humaya Digital\" height=\"40\" style=\"height:40px;max-height:40px;width:auto;display:block;border:0;\" />"
                : "<span style=\"font-size:17px;font-weight:700;letter-spacing:.4px;color:#ffffff;\">HUMAYA&nbsp;DIGITAL</span>";

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
                    "<div style=\"font-size:11px;color:#8d9688;text-transform:uppercase;letter-spacing:.5px;font-weight:600;margin-bottom:6px;\">Motivo</div>" +
                    "<div style=\"background:#fffdf2;border-left:3px solid #fedb05;border-radius:6px;padding:12px 14px;color:#4f5a4b;font-size:14px;line-height:1.5;\">" +
                        datos.Comentario +
                    "</div>" +
                  "</div>"
                : "";

            string etiquetaHtml = !string.IsNullOrWhiteSpace(datos.Etiqueta)
                ? "<span style=\"display:inline-block;background:" + datos.EtiquetaFondo + ";color:" + datos.EtiquetaColor + ";border:1px solid " + datos.EtiquetaBorde + ";" +
                    "font-size:12px;font-weight:700;letter-spacing:.4px;padding:6px 14px;border-radius:20px;\">" + datos.Etiqueta + "</span>"
                : "";

            string html =
            "<html><body style=\"margin:0;padding:0;background-color:#f7f8f6;font-family:Segoe UI,Roboto,Helvetica,Arial,sans-serif;\">" +

                "<div style=\"width:100%;padding:32px 16px;\">" +
                    "<table role=\"presentation\" align=\"center\" width=\"100%\" style=\"max-width:600px;margin:0 auto;border-collapse:collapse;\"><tr><td>" +

                        "<table role=\"presentation\" width=\"100%\" style=\"border-collapse:collapse;\"><tr>" +
                            "<td bgcolor=\"#2b5b25\" style=\"background-color:#2b5b25;background-image:linear-gradient(135deg,#255220,#367c2b);border-radius:14px 14px 0 0;padding:20px 26px;\">" +
                                "<table role=\"presentation\" width=\"100%\"><tr>" +
                                    "<td style=\"vertical-align:middle;\">" + marca + "</td>" +
                                    "<td style=\"vertical-align:middle;text-align:right;\">" + folioHtml + "</td>" +
                                "</tr></table>" +
                            "</td>" +
                        "</tr></table>" +

                        "<div style=\"background:#ffffff;border-radius:0 0 14px 14px;box-shadow:0 6px 24px rgba(31,41,55,0.08);overflow:hidden;\">" +

                            "<div style=\"padding:26px 26px 6px 26px;\">" +
                                "<div style=\"font-size:11px;letter-spacing:1px;color:#8d9688;text-transform:uppercase;font-weight:600;\">Gestión de Proyectos de TI</div>" +
                                "<h2 style=\"margin:6px 0 14px 0;color:#1f2937;font-size:20px;\">" + datos.TituloPrincipal + "</h2>" +
                                etiquetaHtml +
                            "</div>" +

                            "<div style=\"padding:18px 26px 4px 26px;\">" +
                                "<div style=\"background:#f7f8f6;border:1px solid #eef0ec;border-radius:10px;padding:16px 18px;\">" +
                                    filaDato("Solicitud", datos.NombreSolicitud) +
                                    filaDato("Funcionalidad", datos.NombreFuncionalidad) +
                                    filaDato("Mes programado", datos.MesAnio) +
                                    filaDato(datos.SubtituloPersona ?? "Usuario", datos.NombrePersona) +
                                "</div>" +
                            "</div>" +

                            comentarioHtml +

                            "<div style=\"padding:20px 26px 26px 26px;color:#4f5a4b;font-size:13px;line-height:1.5;\">" +
                                (datos.TextoFooter ?? "Ingresa al sistema de Gestión de Proyectos de TI para ver el detalle completo.") +
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
