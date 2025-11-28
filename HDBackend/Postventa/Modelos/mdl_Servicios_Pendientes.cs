namespace Postventa.Modelos
{
    public class mdl_Servicios_Pendientes
    {
        public int id_registro {  get; set; }
        public string cliente {  get; set; }
        public string direccion {  get; set; }
        public string? contacto { get; set; }
        public string sucursal {  get; set; }
        public string num_serie {  get; set; }
        public string registrado { get; set; }
        public string entregado { get; set; }
        public string primer_uso {  get; set; }
        public string modelo { get; set; }
        public int grupo { get; set; }
        public string fecha_envio { get; set; }
        public string fecha_vigencia { get; set; }
        public string estado { get; set; }
        public string motivo {  get; set; }
        public string mensaje_enviado {  get; set; }
        public string facturado { get; set; }
    }
}
