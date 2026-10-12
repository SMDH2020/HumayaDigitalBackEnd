namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_MarcarEstadoActividad
    {
        public int idactividad { get; set; }
        public string estado { get; set; } = "";   // Pendiente | En progreso | Terminada
    }
}
