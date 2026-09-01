namespace HD_Cobranza.Modelos.RecuperacionCartera
{
    public class RC_mdl_totales
    {
        public int idtitulo { get; set; }
        public double saldo { get; set; }
        public double recuperado { get; set; }
        public double porrecuperado => Math.Round(recuperado / saldo * 100);
    }
}
