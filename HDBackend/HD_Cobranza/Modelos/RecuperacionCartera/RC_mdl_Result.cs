namespace HD_Cobranza.Modelos.RecuperacionCartera
{
    public class RC_mdl_Result
    {
        public string encabezado { get; set; }
        public IEnumerable<RC_mdl_titulos> titulos { get; set; }
        public IEnumerable<RC_mdl_detalle> detalle { get; set; }
        public IEnumerable<RC_mdl_sucursales> sucursales { get; set; }
    }
}
