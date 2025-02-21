namespace HD_Cobranza.Modelos.Dashboard
{
    public class mdl_Dashboard_Reporte_Grafica_Total
    {
        public int idadr {  get; set; }
        public string? adr { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public int idcliente { get; set; }
        public int idcliente_HD { get; set; }
        public string? razon_social { get; set; }
        public string? vencimiento { get; set; }
        public int dias_Vencido { get; set; }
        public int recuperado { get; set; }
        public double saldo { get; set; }
        public double objetivo { get; set; }
    }
}
