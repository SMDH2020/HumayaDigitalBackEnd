using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.ProyeccionesVentas
{
    public class mdl_Filtro_Proyecciones_Desglose_Detalle_HD
    {
        public int ejercicio { get; set; }
        public int ejercicioant { get; set; }
        public string periodos { get; set; }
        public string adr { get; set; }
        public string sucursales { get; set; }
        public string Departamentos { get; set; }
        public int usuario { get; set; }
        public string escenario { get; set; }
        public string tipo { get; set; }
        public float importe_proy { get; set; }

    }
}
