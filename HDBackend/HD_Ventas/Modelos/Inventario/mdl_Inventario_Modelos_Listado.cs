namespace HD_Ventas.Modelos.Inventario
{
    public class mdl_Inventario_Modelos_Listado
    {
        public int idGeneral { get; set; }
        public string? linea { get; set; } = "";
        public string? modelo { get; set; } = "";
        public string? descripcion { get; set; } = "";
        public int? departamento { get; set; }
        public string? categoria { get; set; } = "";
    }
}
