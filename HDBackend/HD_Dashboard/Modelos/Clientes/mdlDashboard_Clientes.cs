


namespace HD_Dashboard.Modelos.Clientes
{
    public class mdlDashboard_Clientes
    {
        public IEnumerable<mdlDashClientes_Documentos>? documentos { get; set; }
        public IEnumerable<mdlDashClientes_Linea>? linea { get; set; }
        public mdlDashClientes_LineaTotales? totalcredito { get; set; }
        public string? referenciabancaria { get; set; }
        public IEnumerable<mdlDashClientes_Inventario>? inventario { get; set; }
    }
}
