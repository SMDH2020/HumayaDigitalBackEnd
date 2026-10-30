using System;
using System.Collections.Generic;
using System.Text;

namespace HD_Nomina.Modelos.Dashboard
{
    public class mdl_Personal_Departamento
    {
        public int IDDepartamento { get; set; }
        public string Departamento { get; set; }
        public int total { get; set; }
        public decimal porc { get; set; }
        public decimal antiguedad_prom { get; set; }
        public int cant_mujeres { get; set; }
        public int cant_hombres { get; set; }
    }
}
