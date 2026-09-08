namespace HD_Mensajeria.Modelos
{
    public class mdl_Leads_Agente
    {
        public int id_lead { get; set; }
        public int id_usuario { get; set; }
        public string usuario { get; set; }
        public string area { get; set; }
        public int sucursal { get; set; }
        public string descripcion { get; set; }
        public int orden { get; set; }
        public string tipo_usuario { get; set; }
        public bool estatus { get; set; }
        public bool recibe_notificacion { get; set; }
    }
}
