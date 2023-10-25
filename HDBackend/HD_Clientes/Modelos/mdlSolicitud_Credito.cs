namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito
    {
        public string folio { get; set; } = "";

        public int idcliente { get; set; }

        public char tipo_solicitud { get; set; }

        public double importe { get; set; }

        public char estatus { get; set; }


        public string? usuario { get; set; } = "";
    }
}
