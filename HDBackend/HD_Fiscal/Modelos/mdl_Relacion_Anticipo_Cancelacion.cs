using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Relacion_Anticipo_Cancelacion
    {
        public string serie_fiscal_anticipo { get; set; }
        public string serie_fiscal_cancelacion { get; set; }
        public int usuario { get; set; } = 0;
    }
}
