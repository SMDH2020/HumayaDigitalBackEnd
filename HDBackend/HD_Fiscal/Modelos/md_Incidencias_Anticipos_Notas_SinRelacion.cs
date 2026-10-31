using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Correccion_Incidencias_Anticipos_Notas_SinRelacion
    {
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public float cargo { get; set; }
        public float abono { get; set; }
        public string fecha { get; set; }
        public string batch { get; set; }
        public string document_no { get; set; }
        public string serie_fiscal { get; set; }
        public string v_desc { get; set; }
        public string v_ref { get; set; }
        public string v_usuario { get; set; }

    }
}
