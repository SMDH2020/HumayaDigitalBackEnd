namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Objecion_Pago
    {
        public int id_Objecion {  get; set; }
        public string? descripcion {  get; set; }
        public bool estatus { get; set; }
        public int createuser { get; set; }
        public string? createdate { get; set; }
        public int updateuser { get; set; }
        public string? updatedate { get; set; }
    }
}
