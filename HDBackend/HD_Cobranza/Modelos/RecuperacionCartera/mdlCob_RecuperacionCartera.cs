namespace HD_Cobranza.Modelos.RecuperacionCartera
{
    public class mdlCob_RecuperacionCartera
    {
        public int idsucursal { get; set; }
        public int idadr { get; set; }
        public string? sucursal { get; set; }
        public double vencido { get; set; }
        public double recuperado_vencido { get; set; }
        public double por_vencido { get; set; }
        public double porvencer { get; set; }
        public double recuperado_porvencer { get; set; }
        public double por_porvencer { get; set; }
        public double activo { get; set; }
        public double recuperado_activo { get; set; }
        public double por_activo { get; set; }
    }
}
