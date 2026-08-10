namespace HD.Notifications.SeguimientoActividades
{
    // Correo de "ticket creado" (estatus = C) y de "cambio de estatus"
    // (P/F/A/R) del módulo de Seguimiento de Actividades.
    //
    // Importante: esta clase YA NO decide a quién avisar -- antes siempre
    // mandaba el correo a los responsables de la sala sin importar el
    // caso, lo cual estaba bien para la creación del ticket pero era
    // incorrecto para el cambio de estatus (ahí quien debe enterarse es
    // quien levantó el ticket, no los mismos responsables que acaban de
    // hacer el cambio). Ahora el controller arma la lista de destinatarios
    // según el caso y la pasa explícita -- ver SeguimientoActController.
    public static class NotificacionSeguimientoAct
    {
        public static Task<bool> Enviar(mdlSeguimiento_Email datos, List<string> destinatarios)
        {
            bool esCreacion = datos.estatus == "C";

            var datosCorreo = new DatosCorreoSeguimientoAct
            {
                TituloPrincipal = esCreacion ? "Nuevo ticket de soporte" : "Actualización de tu ticket",
                Folio = datos.folio,
                Actividad = datos.actividad,
                Sala = datos.nombreSala,
                Comentario = datos.comentarios,
                NombrePersona = datos.accionPor ?? datos.usuario,
                SubtituloPersona = esCreacion ? "Reportado por" : "Actualizado por",
                Estatus = datos.estatus,
            };

            string asunto = (esCreacion ? "Nuevo ticket de soporte" : "Actualización de tu ticket")
                + (string.IsNullOrWhiteSpace(datos.folio) ? "" : " · Folio " + datos.folio);

            string html = PlantillaCorreoSeguimientoAct.Renderizar(datosCorreo, EnvioCorreoSeguimientoAct.LogoDisponible());

            return EnvioCorreoSeguimientoAct.Enviar(asunto, html, destinatarios);
        }
    }
}
