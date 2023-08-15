namespace HD_Fiscal.Modelos
{
    public class mdlBancos
    {
        public int idbanco { get; set; }
        public string nombre { get; set; }
        public string cuenta { get; set; }
        public string sucursal { get; set; }
        public string moneda{ get; set; }
        public string clave_interbancaria { get; set; }
        public bool estatus { get; set; }
        public int usuario { get; set; }
    }
}
