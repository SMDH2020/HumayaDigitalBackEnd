using System;
using System.Collections.Generic;
using System.Text;

namespace HD_Nomina.Modelos.Dashboard
{
    public class mdl_Headers_Dash
    {
        public int total_empleados { get; set; }
        public decimal antiguedad_prom { get; set; }
        public int cant_mujeres { get; set; }
        public int cant_hombres { get; set; }
        public int contrataciones { get; set; }
        public int bajas { get; set; }

    }
}
