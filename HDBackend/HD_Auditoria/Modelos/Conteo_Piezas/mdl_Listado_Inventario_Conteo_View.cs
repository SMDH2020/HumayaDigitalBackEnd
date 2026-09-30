namespace HD_Auditoria.Modelos.Conteo_Piezas
{
    public class mdl_Listado_Inventario_Conteo_View
    {
        public mdl_Listado_Inventario_Conteo_Header? header { get; set; }
        public mdl_Listado_Inventario_Conteo_Header_KPIs? kpis { get; set; }
        public List<mdl_Listado_Inventario_Conteo_Piezas>? listado_inv { get; set; }
        public List<mdl_Posicion_Extra>? posiciones_extra { get; set; }
    }
}
