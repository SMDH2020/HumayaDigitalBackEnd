namespace HD_Buro.Modelos
{
    public class mdl_Buro_Credito_Reporte
    {
        public  string?  idcliente { get; set; }
        public  string?  razonsocial { get; set; }
        public  string?  rfc { get; set; }
        public int rev_fac_vencidas { get; set; }
        public double rev_vencido { get; set; }
        public double rev_porvencer { get; set; }
        public int op_fac_vencidas { get; set; }
        public double op_vencido { get; set; }
        public double op_porvencer { get; set; }
        public bool domicilio { get; set; }
        public bool registrado { get; set; }

    }
}
