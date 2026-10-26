using HD.Notifications.Modelos;

namespace HD.Notifications.GestionProyectosTI
{
    // Los 5 correos automaticos de Gestion de Proyectos de TI. El
    // controller (HD_Endpoints) decide a quien avisar (ya resuelto con los
    // AD de solo lectura del proyecto HD_GestionProyectosTI) y llama al
    // metodo correspondiente; esta clase solo arma el contenido y delega el
    // envio a EnvioCorreoTI -- misma separacion que ya usa
    // NotificacionSeguimientoAct para Seguimiento de Actividades.
    public static class NotificacionTI
    {
        private static readonly string[] NombresMes =
        {
            "", "enero", "febrero", "marzo", "abril", "mayo", "junio",
            "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre",
        };

        private static string? MesAnio(byte? mes, short? anio)
        {
            if (mes == null || anio == null || mes < 1 || mes > 12) return null;
            return NombresMes[mes.Value] + " de " + anio.Value;
        }

        // 1) Un usuario crea una solicitud -> avisa a Jefatura de TI.
        public static Task<bool> EnviarSolicitudCreada(mdl_Correo_M365 config, string folio, string titulo, string solicitanteNombre, List<string> destinatarios)
        {
            var datos = new DatosCorreoTI
            {
                TituloPrincipal = "Nueva solicitud de TI",
                Folio = folio,
                Etiqueta = "SOLICITUD NUEVA",
                EtiquetaColor = "#1f3a5f", EtiquetaFondo = "#e8f0fb", EtiquetaBorde = "#3c587c",
                NombreSolicitud = titulo,
                NombrePersona = solicitanteNombre,
                SubtituloPersona = "Solicitante",
                TextoFooter = "Ingresa al sistema para revisar la solicitud y continuar con el proceso de autorización.",
            };
            string asunto = "Nueva solicitud de TI · Folio " + folio;
            string html = PlantillaCorreoTI.Renderizar(datos, EnvioCorreoTI.LogoDisponible());
            return EnvioCorreoTI.Enviar(config, asunto, html, destinatarios);
        }

        // 2) Las dos aprobaciones del alcance se completan -> avisa al
        // solicitante y a Jefatura de TI.
        public static Task<bool> EnviarAlcanceAprobado(mdl_Correo_M365 config, string folio, string titulo, List<string> destinatarios)
        {
            var datos = new DatosCorreoTI
            {
                TituloPrincipal = "Alcance aprobado",
                Folio = folio,
                Etiqueta = "ALCANCE APROBADO",
                EtiquetaColor = "#255220", EtiquetaFondo = "#edf7eb", EtiquetaBorde = "#97c459",
                NombreSolicitud = titulo,
                TextoFooter = "El alcance de esta solicitud ya cuenta con la aprobación del solicitante y de Jefatura de TI. Ingresa al sistema para continuar con la priorización.",
            };
            string asunto = "Alcance aprobado · Folio " + folio;
            string html = PlantillaCorreoTI.Renderizar(datos, EnvioCorreoTI.LogoDisponible());
            return EnvioCorreoTI.Enviar(config, asunto, html, destinatarios);
        }

        // 3) Jefatura de TI prioriza (programa mes/año) una funcionalidad ->
        // avisa al solicitante.
        public static Task<bool> EnviarFuncionalidadPriorizada(mdl_Correo_M365 config, string folio, string titulo, string funcionalidadNombre, byte? mes, short? anio, List<string> destinatarios)
        {
            var datos = new DatosCorreoTI
            {
                TituloPrincipal = "Funcionalidad priorizada",
                Folio = folio,
                Etiqueta = "PRIORIZADA",
                EtiquetaColor = "#255220", EtiquetaFondo = "#edf7eb", EtiquetaBorde = "#97c459",
                NombreSolicitud = titulo,
                NombreFuncionalidad = funcionalidadNombre,
                MesAnio = MesAnio(mes, anio),
                TextoFooter = "Jefatura de TI programó esta funcionalidad. Ingresa al sistema para ver el detalle.",
            };
            string asunto = "Funcionalidad priorizada · Folio " + folio;
            string html = PlantillaCorreoTI.Renderizar(datos, EnvioCorreoTI.LogoDisponible());
            return EnvioCorreoTI.Enviar(config, asunto, html, destinatarios);
        }

        // 4) Jefatura de TI envia una funcionalidad a revision -> avisa al
        // solicitante (quien debe aprobar/rechazar).
        public static Task<bool> EnviarFuncionalidadEnRevision(mdl_Correo_M365 config, string folio, string titulo, string funcionalidadNombre, byte? mes, short? anio, List<string> destinatarios)
        {
            var datos = new DatosCorreoTI
            {
                TituloPrincipal = "Funcionalidad en revisión",
                Folio = folio,
                Etiqueta = "EN REVISIÓN",
                EtiquetaColor = "#8a6d00", EtiquetaFondo = "#fff8de", EtiquetaBorde = "#fedb05",
                NombreSolicitud = titulo,
                NombreFuncionalidad = funcionalidadNombre,
                MesAnio = MesAnio(mes, anio),
                TextoFooter = "Esta funcionalidad quedó en revisión. Ingresa al sistema para aprobarla o rechazarla.",
            };
            string asunto = "Funcionalidad en revisión · Folio " + folio;
            string html = PlantillaCorreoTI.Renderizar(datos, EnvioCorreoTI.LogoDisponible());
            return EnvioCorreoTI.Enviar(config, asunto, html, destinatarios);
        }

        // 5) El solicitante aprueba o rechaza la revision de una
        // funcionalidad -> avisa a Jefatura de TI.
        public static Task<bool> EnviarRevisionResuelta(mdl_Correo_M365 config, string folio, string titulo, string funcionalidadNombre, string solicitanteNombre, bool aprobada, string? comentario, List<string> destinatarios)
        {
            var datos = new DatosCorreoTI
            {
                TituloPrincipal = aprobada ? "Revisión aprobada" : "Revisión rechazada",
                Folio = folio,
                Etiqueta = aprobada ? "REVISIÓN APROBADA" : "REVISIÓN RECHAZADA",
                EtiquetaColor = aprobada ? "#255220" : "#791f1f",
                EtiquetaFondo = aprobada ? "#edf7eb" : "#fcebeb",
                EtiquetaBorde = aprobada ? "#97c459" : "#b3261e",
                NombreSolicitud = titulo,
                NombreFuncionalidad = funcionalidadNombre,
                NombrePersona = solicitanteNombre,
                SubtituloPersona = "Resuelto por",
                Comentario = comentario,
                TextoFooter = "Ingresa al sistema para ver el detalle de la solicitud.",
            };
            string asunto = (aprobada ? "Revisión aprobada" : "Revisión rechazada") + " · Folio " + folio;
            string html = PlantillaCorreoTI.Renderizar(datos, EnvioCorreoTI.LogoDisponible());
            return EnvioCorreoTI.Enviar(config, asunto, html, destinatarios);
        }
    }
}
