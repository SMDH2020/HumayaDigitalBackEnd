namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito
    {
        public string folio { get; set; }

        public int consecutivo { get; set; }

        public int idcliente { get; set; }

        public string seguro_cosecha { get; set; }

        public string tipo_solicitud { get; set; }

        public double importe { get; set; }

        public string estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
