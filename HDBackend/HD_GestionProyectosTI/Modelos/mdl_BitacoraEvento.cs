namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_BitacoraEvento
    {
        public int idbitacora { get; set; }
        public string entidad { get; set; } = "";
        public int identidad { get; set; }
        public int idusuario { get; set; }
        public string campo { get; set; } = "";
        public string? valor_anterior { get; set; }
        public string? valor_nuevo { get; set; }
        public string tipo_evento { get; set; } = "";
        public string? motivo { get; set; }
        public DateTime fecha_hora { get; set; }
    }
}
