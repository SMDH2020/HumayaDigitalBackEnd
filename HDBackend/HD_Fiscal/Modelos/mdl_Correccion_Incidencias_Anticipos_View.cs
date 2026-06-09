namespace HD.Fiscal.Modelos
{
    public class mdl_Correccion_Incidencias_Anticipos_View
    {
        public IEnumerable<mdl_Incidencias_Anticipos_NoLigados_Factura> Anticipos_NoLigados { get; set; }
        public IEnumerable<mdl_Incidencias_Anticipos_Notas_NoTimbradas_ComoEgreso> Notas_NoTimbradas_ComoEgreso { get; set; }
        public mdl_Conciliacion_Ingresos_Analitica_Botones botones { get; set; }
    }
}
