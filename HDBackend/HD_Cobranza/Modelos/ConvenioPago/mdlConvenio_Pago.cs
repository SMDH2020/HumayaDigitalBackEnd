namespace HD_Cobranza.Modelos.ConvenioPago
{
    public class mdlConvenio_Pago
    {
        public string? folio { get; set; }
        public int idcliente { get; set; }
        public string? razon_social { get; set; }
        public double monto { get; set; }
        public DateTime fecha_convenio{ get; set; }
        public bool recordatorio { get; set; }
        public DateTime? fecha_recordatorio { get; set; }
        public string? usuario { get; set; }
    }
}
