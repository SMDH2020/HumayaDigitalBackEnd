namespace HD_Cobranza.Modelos
{
    public class mdlComentarios_Clientes_Contacto
    {
        public int idContacto_Comentario { get; set; }
        public int idcliente { get; set; }
        public int consecutivo { get; set; }
        public string? comentarios { get; set; }
        public int usuario { get; set; }
    }
}
