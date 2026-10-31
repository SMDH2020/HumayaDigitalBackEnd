namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_Estimacion
    {
        public long EstimacionId { get; set; }
        public long SolicitudId { get; set; }
        public long VersionAlcanceId { get; set; }
        public int NumeroVersion { get; set; }
        public string EstadoEstimacion { get; set; } = "";
        public decimal HorasAnalisis { get; set; }
        public decimal HorasDesarrollo { get; set; }
        public decimal HorasPruebas { get; set; }
        public decimal HorasDespliegue { get; set; }
        public decimal HorasTotal { get; set; }
        public string? Riesgos { get; set; }
        public string? Supuestos { get; set; }
        public string? Dependencias { get; set; }
        public int? DiasCalendarioEstimados { get; set; }
        public int RegistradaPorUsuarioId { get; set; }
        public DateTime FechaEstimacion { get; set; }
        public DateTime? FechaValidacion { get; set; }

        public List<mdl_TI_EstimacionDetalleItem>? Detalle { get; set; }
    }
}
