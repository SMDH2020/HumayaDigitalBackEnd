using System;
using System.Collections.Generic;
using System.Text;

namespace HD_Nomina.Modelos.Dashboard
{
    public class mdl_Dashboard_View
    {
        public mdl_Headers_Dash? headers { get; set; }
        public List<mdl_Personal_Sucursal>? personal_sucursal { get; set; }
        public List<mdl_Personal_Departamento>? personal_departamento { get; set; }
        public mdl_Distribucion_Antiguedad? distribucion_antiguedad { get; set; }
        public List<mdl_Contrataciones_Mes>? contrataciones_mes { get; set; }
        public List<mdl_Estructura_Jefe>? estructura_jefes { get; set; }
        public List<mdl_Empleados_Puesto>? empleados_Puestos { get; set; }
        public List<mdl_Listado_Empleados>? listado_empleado { get; set; }

    }
}
