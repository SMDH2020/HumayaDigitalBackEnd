using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace HD_Cobranza.Modelos.IndicadoresRecuperacion
{
    public class mdl_Indicadores_Recuperacion
    {
        public int idIndicador { get; set; }
        public string? tipo_indicador { get; set; }
        public string? tipo_cartera { get; set; }
        public int ejercicio { get; set; }
        public int enero { get; set; }
        public int febrero { get; set; }
        public int marzo { get; set; }
        public int abril { get; set; }
        public int mayo { get; set; }
        public int junio { get; set; }
        public int julio { get; set; }
        public int agosto { get; set; }
        public int septiembre { get; set; }
        public int octubre { get; set; }
        public int noviembre { get; set; }
        public int diciembre { get; set; }
        public bool autoriza_gerencia_finanzas { get; set; }
        public bool autoriza_gerencia_cobranza { get; set; }
        public string? usuario { get; set; }
    }
}
