namespace HD_Buro.Modelos
{
    public class mdlCarga_Detalle_Buro
    {
        public string? modulo {  get; set; }
        public float saldo_operacion {  get; set; }
        public float saldo_revolvente { get; set; }
        public DateTime? fecha_factura { get; set; }
        public DateTime? fecha_vencimiento { get; set; }
        public int dias_vencido {  get; set; }
        public string? linea_credito { get; set; }
        public string? folio_solicitud { get; set; }
        public string? clave { get; set; }
    }
    public class mdlBuroMarcadosResult  {
        public IEnumerable<mdlCarga_Detalle_Buro> operacion{ get; set; }
        public IEnumerable<mdlCarga_Detalle_Buro> revolvente{ get; set; }
    }
}
