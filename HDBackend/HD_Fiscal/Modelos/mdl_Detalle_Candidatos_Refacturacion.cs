using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Detalle_Candidatos_Refacturacion
    {
        public string document_no { get; set; }
        public int cliente { get; set; }
        public float importe_venta { get; set; }
        public string serie { get; set; }
        public string folio { get; set; }
        public string batch { get; set; }
    }
}
