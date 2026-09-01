namespace HD.Clientes.Modelos.CRM.IndicadoresVisitas
{
    /// <summary>
    /// Renglon del reporte de objetivo vs visitas realizadas, por asesor y semana.
    /// cumplimiento_vp es nullable: llega NULL cuando el objetivo es 0 y el front
    /// lo pinta como N/A.
    /// </summary>
    public class mdl_IndicadoresVisitas_ReporteVisitas
    {
        public int idsemana { get; set; }
        public string? semana { get; set; } = "";
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public int idvendedor { get; set; }
        public string? vendedor { get; set; } = "";
        public decimal objetivo_mensual { get; set; }
        public decimal objetivo_semanal { get; set; }
        public int realizadas { get; set; }
        public decimal? cumplimiento_vp { get; set; }
        public int idestado { get; set; }
        public string? estado { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }



    }
}
