namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Detalle
    {
        public string? folio { get; set; }
        public string? tipo_solicitud { get; set; }
        public double importe { get; set; }
        public string? estatus { get; set; }
        public int nivel_riesgo { get; set; }
        public string? autoriza_credito { get; set; }
    }
}
