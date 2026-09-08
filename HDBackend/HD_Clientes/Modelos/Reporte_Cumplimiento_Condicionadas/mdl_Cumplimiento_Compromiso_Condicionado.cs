using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace HD.Clientes.Modelos.Reporte_Cumplimiento_Condicionadas
{
    public class mdl_Cumplimiento_Compromiso_Condicionado
    {
        public string? titulo { get; set; }
        public int ejercicio { get; set; }
        public int sucursal { get; set; }
        public int total_condicionado { get; set; }
        public double porcentaje_documentos_entregados { get; set; }
        public int condicionado_davidt { get; set; }
        public double porcentaje_condicionado_davidt { get; set; }
        public int condicionado_ivanl { get; set; }
        public double porcentaje_condicionado_ivanl { get; set; }
        public int condicionado_armandog { get; set; }
        public double porcentaje_condicionado_armandor { get; set; }
        public int condicionado_mauricior { get; set; }
        public double porcentaje_condicionado_mauricior { get; set; }

    }
}
