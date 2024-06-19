namespace HD_Buro.Modelos
{
    public class mdlCarga_Reporte_Buro
    {
        public int idcliente { get; set; }
        public string? razon_social {  get; set; }
        public string? rfc {  get; set; }
        public int fac_opercion { get; set; }
        public int fac_o_vencidas { get; set; }
        public int fac_o_porvencer {  get; set; }
        public float saldo_operacion { get; set; }
        public int fac_revolvente { get; set; }
        public int fac_r_vencidas {  get; set; }
        public int fac_r_porvencer {  get; set; }
        public float saldo_revolvente { get; set; }
        public bool registrado { get; set; }
        public bool solicitud { get; set; }
        public bool domicilio { get; set; }
        public bool reporta_buro { get; set; }

    }
}
