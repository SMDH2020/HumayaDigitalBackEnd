namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Guardar_Contacto_Cliente
    {
        public int idmedio { get; set; }
        public int idcliente { get; set; }
        public string? tipomedio { get; set; }
        public string? mediocontacto { get; set; } = "";
        public string? medio { get; set; }
        public string? comentarios { get; set; } = "";
        public string? usuario { get; set; }
    }
}
