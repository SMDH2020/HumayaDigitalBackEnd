using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Listado_rel_anticipos
    {
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public string folio { get; set; }
        public string serie { get; set; }
        public string serie_fiscal { get; set; }
        public float cargo { get; set; }
        public float abono { get; set; }
        public string fecha { get; set; }
        public string batch { get; set; }
        public string usuario { get; set; }
        public bool chk { get; set; }
        public bool bloqueado { get; set; }
        public string? estatus { get; set; }

    }
}
