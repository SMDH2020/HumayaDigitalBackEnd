namespace HD_Dashboard.Modelos.Clientes
{
    public class mdlDashClientes_Facturacion_Detalle
    {
        public string? sucursal { get; set; }
        public string? linea { get; set; }
        public string? serie { get; set; }
        public string? folio { get; set; }
        public string? fecha { get; set; }
        public string? vencimiento { get; set; }
        public double vencido { get; set; }
        public double porvencer { get; set; }
        public double importe { get; set; }
    }
}
