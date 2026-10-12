namespace HD.Clientes.Modelos.Clientes_Juridico
{
    public class mdl_Clientes_Juridico
    {
        public int idregistro {  get; set; }
        public int idcliente {  get; set; }
        public string? razon_social { get; set; }
        public string? rfc { get; set; }
        public float importe_total_vencido {  get; set; }
        public int dias_vencido { get; set; }
        public string? fecha_juridico { get; set; }
        public string? domicilio {  get; set; }
        public string? telefono {  get; set; }
        public string? estatus_cliente {  get; set; }
        public int num_expediente { get; set; }
        public string? nombre_juzgado {  get; set; }
        public string? estatus_demanda {  get; set; }

    }
}
