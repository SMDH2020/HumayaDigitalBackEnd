namespace HD_Mensajeria.Modelos
{
    public class mdl_Contactos_Mensajeria_Menu
    {
        public string telefono {  get; set; }
        public string ultimo_mensaje {  get; set; }
        public string cliente { get; set; }
        public string mensaje { get; set; }
        public string mensajePlantilla { get; set; }
        public int sucursal {  get; set; }
        public string modulo { get; set; }
        public bool alerta { get; set; }
        public bool atendido { get; set; }
        public bool sin_respuesta { get; set; }
    }
}
