using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Reporte_Cumplimiento_Condicionadas
{
    public class mdl_Cumplimiento_Compromiso_Condicionado_General
    {
        public string? folio { get; set; }
        public string? razon_social { get; set; }
        public int idvendedor { get; set; }
        public string? vendedor { get; set; }
        public int documentos_solicitados { get; set; }
        public int documentos_entregados { get; set; }
        public double porcentaje_documentos_entregados { get; set; }
        public int documentos_puntuales { get; set; }
        public double porcentaje_documentos_puntuales { get; set; }
        public int documentos_retrasados { get; set; }
        public double porcentaje_documentos_retrasados { get; set; }
        public int idgerente { get; set; }
        public string gerente { get; set; }
        public int idmes { get; set; }
        public string? mes { get; set; }
    }
}
