namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Balance_Patrimonial
    {
        public string folio { get; set; }

        public short registro { get; set; }

        public string concepto { get; set; }

        public string tipo { get; set; }

        public double importe { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
