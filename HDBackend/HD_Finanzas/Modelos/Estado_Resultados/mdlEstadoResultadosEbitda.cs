namespace HD_Finanzas.Modelos.Estado_Resultados
{
    public class mdlEstadoResultadosEbitda
    {
        public int index { get; set; }
        public string? departamento{ get; set; }
        public string? concepto { get; set; }
        public int orden { get; set; }
        public float importe { get; set; }
        public float por { get; set; }
        public float proyimporte { get; set; }
        public float proypor { get; set; }
        public string indicador { get; set; }
        public float diffimporte { get; set; }
        public float diffpor { get; set; }
        public float lastimporte { get; set; }
        public float lastpor { get; set; }
        public float lastdiffimporte { get; set; }
        public float lastdiffpor { get; set; }
        public string clase { get; set; }
    }
}
