namespace HD_GestionProyectosTI.Modelos
{
    // Usado como item de entrada (detalle al crear) y como fila de salida
    // (TI_EstimacionDetalle) -- las columnas que necesita cada uso coinciden.
    public class mdl_TI_EstimacionDetalleItem
    {
        public long FuncionalidadId { get; set; }
        public decimal HorasAnalisis { get; set; }
        public decimal HorasDesarrollo { get; set; }
        public decimal HorasPruebas { get; set; }
        public decimal HorasDespliegue { get; set; }
        public string? Observaciones { get; set; }
    }
}
