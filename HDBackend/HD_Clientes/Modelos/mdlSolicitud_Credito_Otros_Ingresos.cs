namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Otros_Ingresos
    {
        public string folio { get; set; }

        public short registro { get; set; }

        public string fuente { get; set; }

        public double ingresos { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
