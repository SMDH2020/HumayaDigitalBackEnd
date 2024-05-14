namespace HD.Clientes.Modelos.Facturacion
{
    public class mdlFAC_FacturasByFolio
    {
        public long idfactura { get; set; }
        public string serie { get; set; } = "";
        public string folio { get; set; } = "";
        public string cliente { get; set; } = "";
        public string vendedor { get; set; } = "";
        public string documento { get; set; } = "";
        public DateTime fechasuscripcion { get; set; } = DateTime.Now;
        public string dtpfechasuscripcion => fechasuscripcion.ToString("yyyy-MM-dd");
        public bool tieneinteres { get; set; } = false;
        public double tasa { get; set; } = 0;
        public double intereses { get; set; } = 0;
        public string? usuario { get; set; }
        public double montofinanciado { get; set; } = 0;
        public double montointereses { get; set; } = 0;
        public double interesdiario => diasfinanciamiento == 0 || intereses == 0 ? 0 : diasfinanciamiento / intereses;
        public int diasfinanciamiento { get; set; } = 0;
        public string? financiera { get; set; }
        public DateTime vencimiento { get; set; }
        public string dtpvencimiento => vencimiento.ToString("yyyy-MM-dd");
    }
}
