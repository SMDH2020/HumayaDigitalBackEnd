namespace HD_Finanzas.Modelos.Gastos_Proyeccion
{
    public class Fmdl_GastosPorConcepto
    {
        public string? tipo { get; set; }
        public string? cuenta { get; set; }
        public string? concepto { get; set; }
        public double total { get; set; }
        public double proyeccion { get; set; }
        public double porc { get; set; }
        public double dif { get; set; }
        public double oldtotal { get; set; }
        public double oldproyeccion { get; set; }
        public double oldporc { get; set; }
        public double olddif { get; set; }
    }
}
