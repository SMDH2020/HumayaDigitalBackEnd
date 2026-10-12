namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_CambioFecha
    {
        public int idsolicitud { get; set; }
        public DateTime fecha_nueva { get; set; }
        public string motivo { get; set; } = "";   // siempre obligatorio
    }
}
