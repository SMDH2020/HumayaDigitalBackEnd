namespace HD_Cobranza.Modelos.RecuperacionCartera
{
    public class RC_mdl_detalle
    {
        public int id { get; set; }
        public int idtitulo { get; set; }
        public string cartera { get; set; }
        public int idadr { get; set; }
        public string adr { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public double saldo { get; set; }
        public double recuperado { get; set; }
    }
}
