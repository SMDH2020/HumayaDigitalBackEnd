namespace HD_Finanzas.Modelos.Margenes
{
    public class mdlMargenes_Detalle_Completo
    {
        public string? familia { get; set; }
        public int ordenfamilia { get; set; }
        public string? linea { get; set; }
        public int orden { get; set; }
        public string? fecha { get; set; }
        public string? serie { get; set; }
        public string? folio { get; set; }
        public string? vendedor { get; set; }
        public string? sucursal { get; set; }
        public string? neconomico { get; set; }
        public string? modelo { get; set; }
        public string? cliente { get; set; }
        public string? razonsocial { get; set; }
        public string? condicion { get; set; }
        public double ventaneta { get; set; }
        public double costo_total { get; set; }
        public double margen { get; set; }
    }
}
