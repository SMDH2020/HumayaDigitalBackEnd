namespace HD_Dashboard.Modelos.Clientes
{
    public class mdlDashClientes_Linea
    {
        public string? linea{ get; set; }
        public double porvencer { get; set; }
        public double vencido { get; set; }
        public double importe { get; set; }
    }
    public class mdlDashClientes_LineaTotales
    {
        public double porvencer { get; set; }
        public double vencido { get; set; }
        public double total { get; set; }
    }

}
