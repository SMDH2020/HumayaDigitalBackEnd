namespace HD_Ventas.Modelos
{
    public class mdl_Agregar_Cotizacion
    {
        public int idcliente {  get; set; }
        public int idasesor { get; set; }
        public string tipo_pago { get; set; }
        public string vigencia { get; set; }
        public int usuario { get; set; }
        public string detalle { get; set; }
    }
}
