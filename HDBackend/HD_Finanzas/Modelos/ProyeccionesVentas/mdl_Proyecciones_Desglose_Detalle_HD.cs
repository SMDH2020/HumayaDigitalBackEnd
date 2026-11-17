using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.ProyeccionesVentas
{
    public class mdl_Proyecciones_Desglose_Detalle_HD
    {
        public string cuenta { get; set; }
        public string concepto { get; set; }
        public string tipo { get; set; }
        public string tipo_gasto { get; set; }
        public float importe { get; set; }
        public float proyeccion { get; set; }
    }
}
