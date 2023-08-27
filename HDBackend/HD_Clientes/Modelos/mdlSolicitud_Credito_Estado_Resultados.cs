namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Estado_Resultados
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
