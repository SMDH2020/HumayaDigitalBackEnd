namespace HD_Finanzas.Modelos.NivelInventario
{
    public class mdl_Nivel_Inventario_Filtrado
    {
        public short Ejercicio { get; set; }
        public short Periodo { get; set; }
        public string adr { get; set; }
        public string sucursal { get; set; }
        public string departamentos {  get; set; }
        public string? seleccion { get; set; }
        public string usuario { get; set; }
    }
}
