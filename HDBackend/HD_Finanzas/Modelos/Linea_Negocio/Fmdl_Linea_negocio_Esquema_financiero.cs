namespace HD_Finanzas.Modelos.Linea_Negocio
{
    public class Fmdl_Linea_negocio_Esquema_financiero
    {
        public Fmdl_Linea_negocio_ventas_netas_totales? ventastotales { get; set; }
        public Fmdl_Linea_negocio_margenes_por_grupo? margenesbygrupo { get; set; }
        public  IEnumerable<Fmdl_Linea_negocio_estado_resultados>?  estadoresultados { get; set; }
        public Fmdl_Linea_negocio_Indicador_margenes? indicadores { get; set; }
        public Fmdl_Linea_negocio_ventas_netas_vs_totales? margenventasnetas { get; set; }
    }
}
