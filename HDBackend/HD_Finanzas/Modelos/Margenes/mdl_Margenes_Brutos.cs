namespace HD_Finanzas.Modelos.Margenes
{
    public class mdl_Margenes_Brutos
    {
        public int idadr {  get; set; }
        public string adr { get; set; }
        public int idsucursal {  get; set; }
        public string sucursal { get; set; }
        public int iddepartamento { get; set; }
        public string departamento { get; set; }
        public float utilidad_bruta {  get; set; }
        public float gasto_Departamento { get; set; }
        public float utilidad_operacion {  get; set; }
    }
}
