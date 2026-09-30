using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Reporte_Cumplimiento_Condicionadas
{
    public class mdl_detalle_folio
    {
        public string? folio { get; set; }
        public string? documento { get; set; }
        public string? fecha_solicitud { get; set; }
        public string? fecha_compromiso { get; set; }
        public string? fecha_entregado { get; set; }
        public int dias_vencido { get; set; }
    }
}
