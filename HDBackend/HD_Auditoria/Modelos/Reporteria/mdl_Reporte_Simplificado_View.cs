
namespace HD_Auditoria.Modelos.Reporteria
{
    public class mdl_Reporte_Simplificado_View
    {
        public IEnumerable<mdl_Reporte_Simplificado_Detalle> detalle { get; set; }
        public mdl_Reporte_Simplificado_Resumen resumen { get; set; }
    }
}
