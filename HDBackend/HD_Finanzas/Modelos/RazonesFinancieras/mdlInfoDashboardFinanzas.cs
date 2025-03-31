using HD_Finanzas.Modelos.CostoFinanciamiento;
using HD_Finanzas.Modelos.NivelInventario;
using HD_Finanzas.Modelos.ResultadosSucursal;

namespace HD_Finanzas.Modelos.RazonesFinancieras
{
    public class mdlInfoDashboardFinanzas
    {
        public mdlEstadoResultadoReal? estadoresultadoreal { get; set; }
        public mdlEstadoResultadoProyectado? estadoresultadoproyectado { get; set; }
        public List<mdlVentasNetas>? ventasnetas { get; set; }
        public List<mdlVentasNetasAnterior>? ventasnetasAnterior { get; set; }
        public List<mdlVentasNetasProyectadas>? ventasnetasproyectada { get; set; }
        public List<mdlGastos>? gastos { get; set; }
        public List<mdlGastosAnterior>? gastosanterior { get; set; }
        public List<mdlGastosProyectados>? gastosproyectados { get; set; }
        public List<mdlBalanceGeneral>? balancegeneral { get; set; }
        public List<mdlRazonesFinancieras>? razonesfinancieras { get; set; }
        public List<mdl_Inventario_Antiguedad_Dash>? InventarioAntiguedad {  get; set; }
        public List<mdl_Costo_Financiamiento_Dash>? CostoFinanciamiento { get; set; }
        public List<mdl_Resultados_Sucursal_Dash>? ResultadosSucursal { get; set; }
        public List<mdl_Nivel_Inventario_Dash>? NivelInventario {  get; set; }
    }
}
