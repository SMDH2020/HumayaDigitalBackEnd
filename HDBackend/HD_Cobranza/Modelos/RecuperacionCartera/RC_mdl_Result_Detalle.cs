namespace HD_Cobranza.Modelos.RecuperacionCartera
{
    public class RC_mdl_Result_Detalle
    {
        public string? serie { get; set; }
        public string? folio { get; set; }
        public int idadr { get; set; }
        public string? adr { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public int idcliente { get; set; }
        public string? razonsocial { get; set; }
        public DateTime fecha{ get; set; }
        public string? strfecha => fecha.ToString("dd/MM/yyyy");
        public double saldo { get; set; }
        public double recuperado { get; set; }
        public double facturado { get; set; }
        public double abono { get; set; }
        public double porrecuperado => Math.Round(abono / saldo * 100);
    }
}
