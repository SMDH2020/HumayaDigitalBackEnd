namespace HD.Clientes.Modelos.Clientes_Juridico
{
    public class mdl_Guardar_Gestion_Judicial
    {
        public int idcliente {  get; set; }
        public int estatus_cliente { get; set; }
        public int expediente {  get; set; }
        public string? juzgado {  get; set; }
        public int estatus_demanda {  get; set; }
        public float importe_convenio { get; set; }
        public string? fecha_convenio { get; set; }
        public string? medio_contacto {  get; set; }
        public string? fecha_visita {  get; set; }
        public string? fecha_liquidacion {  get; set; }
        public float importe_liquidacion_pagado { get; set; }
        public string? fecha_dacion { get; set; }
        public float importe_dacion_valuado { get; set; }
        public string? fecha_devolucion { get; set; }
        public float importe_devolucion_valuado { get; set; }
        public string? apartado { get; set; }
        public int usuario {  get; set; }
    }
}
