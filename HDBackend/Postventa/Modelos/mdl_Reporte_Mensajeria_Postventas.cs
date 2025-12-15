namespace Postventa.Modelos
{
    public class mdl_Reporte_Mensajeria_Postventas
    {
        public int id_registro {  get; set; }
        public string? cliente { get; set; }
        public string? contacto {  get; set; }
        public string? sucursal { get; set; }
        public int id_sucursal { get; set; }
        public string? num_serie {  get; set; }
        public string? modelo {  get; set; }
        public string? estado {  get; set; }
        public string? motivo { get; set; }
        public string? facturado { get; set; }

        public int id_garantia { get; set; }
        public int idestado {  get; set; }
        public string? entregado { get; set; }
        public string? expiracion_format {  get; set; }
        public string? whatsapp {  get; set; }
        public string? estatus { get; set; }

        public string? orden {  get; set; }
        public int idcliente { get; set; }
        public int idcliente_equip {  get; set; }
        public int id_vendedor { get; set; }
        public string? vendedor { get; set; }
        public string? fecha { get; set; }
        public string? fecha_envio { get; set; }
        public string? tipo { get; set; }
    }
}
