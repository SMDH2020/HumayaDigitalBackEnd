namespace HD_Auditoria.Modelos.Conteo_Piezas
{
    public class mdl_Conteo_Piezas_Online_Response
    {
        public int id_conteo {  get; set; }
        public float diferencia { get; set;  }
        public string tipo_dif {  get; set; }
        public int resultado { get; set; }
        public string mensaje { get; set; }
    }
}
