namespace HD_Checador.Modelos
{
    public class mdl_Empleado
    {
        public int IDEmpleado { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public int IDEstado { get; set; }
        public string? Estado { get; set; }
        public int IDSucursal { get; set; }
        public string? Sucursal { get; set; }
        public int IDDepartamento { get; set; }
        public string? Departamento { get; set; }
        public int IDPuesto { get; set; }
        public string? Puesto { get; set; }
        public string? Correo { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public string? Antiguedad { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Sexo { get; set; }
        public string? JEFE { get; set; }
        public double sueldo_diario { get; set; }
        public double sueldo_promedio { get; set; }
        
        public string? cargo_contable { get; set; }
    }
}
