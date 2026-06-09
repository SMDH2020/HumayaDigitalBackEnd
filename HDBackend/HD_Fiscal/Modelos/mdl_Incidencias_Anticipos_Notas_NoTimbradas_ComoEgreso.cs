using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Incidencias_Anticipos_Notas_NoTimbradas_ComoEgreso
    {
        public int idregistro { get; set; }
        public int documento { get; set; }
        public string fecha { get; set; }
        public string serie_fiscal { get; set; }
        public string folio_fiscal { get; set; }
        public int batch { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public int iddepartamento { get; set; }
        public string departamento { get; set; }
        public string cuenta { get; set; }
        public float importe { get; set; }
        public string tipoComprobante { get; set; }
        public string UUID { get; set; }
    }
}
