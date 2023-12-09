namespace HD.Clientes.Modelos.SC_Analisis
{
    public class mdlSC_Analisis_SolicitudCredito
    {
        public string? folio { get; set; }
        public string? cliente { get; set; }
        public string? sucursal { get; set; }
        public string? vendedor { get; set; }
        public string? fecha_creacion { get; set; }
        public string? tipo_solicitud { get; set; }
        public string? linea_credito { get; set; }
        public string? nivel_riesgo { get; set; }
        public string? responsable_credito { get; set; }
        public string? estado { get; set; }
        public double importe { get; set; }
    }
}
