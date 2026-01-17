using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.RotacionInventario
{
    public class mdl_Rotacion_Inventario
    {
        public int orden { get; set; }
        public int id { get; set; }
        public string? linea { get; set; }
        public int idmodulo { get; set; }
        public int minimo { get; set; }
        public int maximo { get; set; }
        public double venta { get; set; }
        public double costo { get; set; }

        public double opt_maximo { get; set; }
        public double opt_minimo { get; set; }
        public double inventario { get; set; }
        public double dif_minima { get; set; }
        public double dif_maxima { get; set; }
        public double rotacion { get; set; }

    }
}
