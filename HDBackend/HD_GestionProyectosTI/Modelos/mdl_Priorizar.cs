namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_Priorizar
    {
        public int idsolicitud { get; set; }
        public string prioridad { get; set; } = "";              // Critica | Alta | Media | Baja
        public DateTime fecha_estimada { get; set; }
        public string priorizado_con { get; set; } = "";         // area/persona de Planeacion Estrategica
        public string? comentario_priorizacion { get; set; }
        // Obligatorio solo si la solicitud ya estaba priorizada (repriorizacion).
        // El controller decide si aplica, consultando el estado actual.
        public string? motivo { get; set; }
    }
}
