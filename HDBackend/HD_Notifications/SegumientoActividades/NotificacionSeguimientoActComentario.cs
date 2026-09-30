namespace HD.Notifications.SeguimientoActividades
{
    // Correo de "nuevo comentario" del módulo de Seguimiento de
    // Actividades. Igual que NotificacionSeguimientoAct, ya no decide a
    // quién avisar -- antes siempre mandaba a los responsables de la sala,
    // así que si el propio responsable comentaba se avisaba a sí mismo y
    // el creador del ticket nunca se enteraba de nada. El controller ahora
    // calcula "la otra parte" (creador <-> responsables) según quién
    // comentó y pasa esa lista explícita -- ver SeguimientoActController.
    public static class NotificacionSeguimientoActComentario
    {
        // paraCreador: true si el destinatario es quien levantó el ticket
        // (comentó el responsable), false si son los responsables de la
        // sala (comentó el creador) -- solo cambia el texto del título.
        public static Task<bool> Enviar(mdlSeguimiento_Email datos, List<string> destinatarios, bool paraCreador = true)
        {
            var datosCorreo = new DatosCorreoSeguimientoAct
            {
                TituloPrincipal = paraCreador ? "Nuevo comentario en tu ticket" : "Nuevo comentario en un ticket que atiendes",
                Folio = datos.folio,
                Actividad = datos.actividad,
                Sala = datos.nombreSala,
                Comentario = datos.comentarios,
                NombrePersona = datos.accionPor ?? datos.usuario,
                SubtituloPersona = "Comentario de",
                Estatus = "M",
            };

            string asunto = "Nuevo comentario" + (string.IsNullOrWhiteSpace(datos.folio) ? "" : " · Folio " + datos.folio);

            string html = PlantillaCorreoSeguimientoAct.Renderizar(datosCorreo, EnvioCorreoSeguimientoAct.LogoDisponible());

            return EnvioCorreoSeguimientoAct.Enviar(asunto, html, destinatarios);
        }
    }
}
