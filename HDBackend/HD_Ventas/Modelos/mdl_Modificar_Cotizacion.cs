namespace HD_Ventas.Modelos
{
    public class mdl_Modificar_Cotizacion
    {
        public string folio {  get; set; }
        public int idcliente { get; set; }
        public int idasesor { get; set; }
        public int crm { get; set; }
        public string asunto { get; set; }
        public int idsucursal { get; set; }
        public string tipo_pago { get; set; }
        public string fase {  get; set; }
        public string vigencia { get; set; }
        public int usuario { get; set; }
        public string detalle { get; set; }
    }
}
