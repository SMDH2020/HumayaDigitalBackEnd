using System;
using System.Collections.Generic;
using System.Text;

namespace HD_Nomina.Modelos.Dashboard
{
    public class mdl_Listado_Empleados
    {
        public int IDEmpleado { get; set; }
        public string empleado { get; set; }
        public int IDEstado { get; set; }
        public string Estado { get; set; }
        public int IDSucursal { get; set; }
        public string Sucursal { get; set; }
        public int IDDepartamento { get; set; }
        public string Departamento { get; set; }
        public string Puesto { get; set; }
        public string Correo { get; set; }
        public string FechaIngreso { get; set; }   // viene como varchar dd/MM/yyyy
        public decimal antiguedad { get; set; }
        public int Sexo { get; set; }              // 0 = mujer, 1 = hombre
        public string nombre_jefe { get; set; }
        public string cargo_contable { get; set; }
        public float sueldo_diario { get; set; }
        public float sueldo_promedio { get; set; }
    }
}
