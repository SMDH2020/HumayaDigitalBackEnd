using System;
using System.Collections.Generic;
using System.Text;

namespace HD_Nomina.Modelos.Dashboard
{
    public class mdl_Empleados_Puesto
    {
        public string puesto { get; set; }
        public int empleados { get; set; }
        public decimal porc_total { get; set; }
        public int cant_mujeres { get; set; }
        public int cant_hombres { get; set; }

    }
}
