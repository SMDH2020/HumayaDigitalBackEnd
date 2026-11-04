namespace HD_Checador.Modelos
{
    public class mdl_AltasYBajas_View
    {
        public IEnumerable<mdl_Empleado> altas { get; set; } = new List<mdl_Empleado>();
        public IEnumerable<mdl_Empleado> bajas { get; set; } = new List<mdl_Empleado>();
    }
}
