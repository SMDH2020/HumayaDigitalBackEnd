using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.NivelInventario
{
    public class mdl_Nivel_Inventario_Dash
    {
        public double invactual { get; set; }
        public double invanterior { get; set; }
        public int dias { get; set; }
        public int diasant { get; set; }
        public float diferencia {  get; set; }
    }
}
