namespace HD_Ventas.Modelos.Inventario
{
    public class mdl_Inventario_Modelos_Listado_View
    {
        public IEnumerable<mdl_Inventario_Modelos_Listado> inventario { get; set; } = new List<mdl_Inventario_Modelos_Listado>();
        public IEnumerable<mdl_Inventario_Departamentos> departamentos { get; set; } = new List<mdl_Inventario_Departamentos>();
    }
}
