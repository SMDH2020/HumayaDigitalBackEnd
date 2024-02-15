using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Solicitud_Impresion
{
    public class mdl_Solicitud_Estado_Resultados_View
    {
        public string? folio { get; set; }
        public double in_agricolas { get; set; }
        public double in_ganado { get; set; }
        public double in_leche { get; set; }
        public double in_maquilas { get; set; }
        public double in_procampo { get; set; }
        public double in_rentas { get; set; }
        public double in_sueldos { get; set; }
        public double in_otros { get; set; }
        public double eg_agricolas { get; set; }
        public double eg_ganaderos { get; set; }
        public double eg_maquilas { get; set; }
        public double eg_terrenos { get; set; }
        public double eg_refaccionarios { get; set; }
        public double eg_intereses { get; set; }
        public double eg_impuestos { get; set; }
        public double eg_familiares { get; set; }
        public double eg_otros { get; set; }
    }
}
