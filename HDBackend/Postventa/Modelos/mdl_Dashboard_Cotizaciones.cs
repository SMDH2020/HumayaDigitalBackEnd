namespace Postventa.Modelos
{
    public class mdl_Dashboard_Cotizaciones
    {
        public int total_mensajes { get; set; }
        public int pendientes { get; set; }
        public int por_pendientes { get; set; }
        public int mensajes_enviados { get; set; }
        public int por_mensajes_enviados { get; set; }
        public int esperando_respuesta { get; set; }
        public int por_esperando_respuesta { get; set; }
        public int cliente_con_interes { get; set; }
        public int por_cliente_con_interes { get; set; }
        public int cliente_sin_interes { get; set; }
        public int por_cliente_sin_interes { get; set; }
        public int tiempo_entrega { get; set; }
        public int por_tiempo_entrega { get; set; }
        public int precio_refaccion { get; set; }
        public int por_precio_refaccion { get; set; }
        public int atencion { get; set; }
        public int por_atencion { get; set; }
        public int no_necesito { get; set; }
        public int por_no_necesito { get; set; }



        //public string? orden { get; set; }
        //public int idsucursal { get; set; }
        //public string? sucursal { get; set; }
        //public int idcliente { get; set; }
        //public string? cliente { get; set; }
        //public string? vendedor { get; set; }
        //public string? fecha { get; set; }
        //public double total { get; set; }
        //public string? whatsapp { get; set; }
        //public string? estado { get; set; }
        //public string? mensaje1 { get; set; }
        //public string? mensaje2 { get; set; }

    }
    public class mdl_Dashboard_Cotizaciones_list
    {
        public string? orden { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public int idcliente { get; set; }
        public string? cliente { get; set; }
        public string? vendedor { get; set; }
        public string? fecha { get; set; }
        public double total { get; set; }
        public string? whatsapp { get; set; }
        public string? estado { get; set; }
        public string? mensaje1 { get; set; }
        public string? mensaje2 { get; set; }
    }
}
