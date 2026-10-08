namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    /// <summary>
    /// Fila del sexto select del SP: situaciones que requieren accion.
    /// prioridad: critica | alta | media. El id lo asigna el AD despues de ordenar.
    /// </summary>
    public class mdl_Dashboard_Atencion_Fila
    {
        public string? prioridad { get; set; }
        public string? titulo { get; set; }
        public string? descripcion { get; set; }
        public string? jd_org_id { get; set; }
        public int maquina_id { get; set; }

        public DateTime fecha_evento { get; set; }

    }
}
