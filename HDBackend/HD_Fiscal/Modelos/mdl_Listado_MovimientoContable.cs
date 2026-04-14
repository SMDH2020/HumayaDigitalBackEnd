using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Listado_MovimientoContable
    {
        public int idmovimiento { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public int iddepartamento { get; set; }
        public string departamento { get; set; }
        public string cuenta { get; set; }
        public float importe { get; set; }
        public string fecha { get; set; }
        public int batch { get; set; }
        public string usuario { get; set; }
        public bool chk { get; set; }
        public bool bloqueado { get; set; }
    }
}
