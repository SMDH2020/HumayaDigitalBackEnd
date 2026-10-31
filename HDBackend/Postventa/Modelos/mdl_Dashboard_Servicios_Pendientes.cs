namespace Postventa.Modelos
{
    public class mdl_Dashboard_Servicios_Pendientes
    {
        public int total_mensajes {  get; set; }
        public int pendientes { get; set; }
        public float pendientes_porcentaje {  get; set; }
        public int enviados { get; set; }
        public float enviados_porcentaje { get; set; }
        public int leidos { get; set; }
        public float leidos_porcentaje { get; set; }
        public int facturados { get; set; }
        public float facturados_porcentaje { get; set; }
        public int esperando_respuesta { get; set; }
        public int esperando_respuesta_porcentaje { get; set; }
        public int clientes_interes {  get; set; }
        public int clientes_interes_porcentaje { get; set; }
        public int clientes_sin_interes { get; set; }
        public int clientes_sin_interes_porcentaje { get; set; }
        public int mensajes_error {  get; set; }
        public int mensajes_error_porcentaje { get; set; }
        public int precio_servicio { get; set; }
        public int precio_servicio_porcentaje { get; set; }
        public int disponibilidad_credito {  get; set; }
        public int disponibilidad_credito_porcentaje { get; set; }
        public int atencion_brindada {  get; set; }
        public int atencion_brindada_porcentaje { get; set; }
        public int no_necesito { get; set; }
        public int no_necesito_porcentaje { get; set; }
    }
}
