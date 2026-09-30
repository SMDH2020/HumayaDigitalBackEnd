namespace HD.Clientes.Modelos.Facturar_Equipo
{
    public class mdl_Factuar_Equipo_View
    {
        public string? folio { get; set; }
        public string? sucursal { get; set; }
        public string? fecha { get; set; }
        public int idproceso { get; set; }
        public string? proceso { get; set; }
        public int idcliente { get; set; }
        public string? cliente { get; set; }
        public string? vendedor { get; set; }
        public double importe { get; set; }
    }
}
