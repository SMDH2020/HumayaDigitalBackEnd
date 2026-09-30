
namespace HD_Cobranza.Modelos.RecuperacionCartera
{
    public  class RC_mdl_Result_DetalleResult
    {
        public IEnumerable<RC_mdl_Result_Detalle>? data { get; set; }
        public RC_mdl_Result_DetalleTotales? totales { get; set; }
    }
}
