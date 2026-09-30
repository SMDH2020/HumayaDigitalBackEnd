namespace HD_GestionProyectosTI.Modelos
{
    // Input del formulario de captura. idusuario_solicitante lo llena el
    // controller a partir de ISesion, no viene del cliente.
    public class mdl_SolicitudCrear
    {
        public string tipo { get; set; } = "";              // Incidencia | Nuevo desarrollo | Mejora | Acceso a información
        public string titulo { get; set; } = "";
        public string descripcion { get; set; } = "";
        public int idmodulo { get; set; }

        public int idusuario_solicitante { get; set; }

        // Solo aplican (y son obligatorias) cuando tipo = "Acceso a información".
        // El controller valida eso; aquí quedan opcionales para los otros 3 tipos.
        public string? pregunta_informacion { get; set; }
        public string? pregunta_objetivo_negocio { get; set; }
        public string? pregunta_decisiones { get; set; }
        public string? pregunta_frecuencia { get; set; }
        public string? pregunta_uso_compartido { get; set; }

        public bool impacto_control_interno { get; set; }
        public bool impacto_normativo { get; set; }
        public bool impacto_financiero { get; set; }
        public string? impacto_comentario { get; set; }
    }
}
