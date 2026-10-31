namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    public class mdl_Dashboard_Atencion
    {
        public int id { get; set; }
        public string prioridad { get; set; } = "media";
        public string? titulo { get; set; }
        public string? descripcion { get; set; }
        public string? jd_org_id { get; set; }
        public int maquina_id { get; set; }
        public DateTime fecha_evento { get; set; }

    }
}
