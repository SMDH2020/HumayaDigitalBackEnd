using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Programar_Inventario
{
    public class mdl_Finalizacion_Metricas
    {
        public double importe_total_inventario { get; set; }
        public double importe_faltante { get; set; }
        public double porc_faltante { get; set; }
        public double importe_sobrante { get; set; }
        public double porc_sobrante { get; set; }
        public double total_neto { get; set; }
        public double confiabilidad { get; set; }
        public double confiabilidad_ubi { get; set; }


    }
}
