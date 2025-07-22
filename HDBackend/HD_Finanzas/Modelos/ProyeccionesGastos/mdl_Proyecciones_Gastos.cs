using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.ProyeccionesGastos
{
    public class mdl_Proyecciones_Gastos
    {
        public string cuenta { get; set; }
        public string concepto { get; set; }
        public string tipo { get; set; }
        public double proy { get; set; }
        public double total { get; set; }
        public double diferencia { get; set; }
        public double por { get; set; }
    }
}
