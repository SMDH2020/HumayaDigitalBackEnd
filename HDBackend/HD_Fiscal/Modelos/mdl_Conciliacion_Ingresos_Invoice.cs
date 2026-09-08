using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Conciliacion_Ingresos_Invoice
    {
        public string origen { get; set; }
        public string documento { get; set; }
        public string cust_ord_no { get; set; }
        public string fecha { get; set; }
        public int ro_number { get; set; }
        public string special_inst { get; set; }
        public string serie_fiscal { get; set; }
        public string folio_fiscal { get; set; }
        public string batch { get; set; }
        public string cuenta { get; set; }
        public string uuid { get; set; }
        public string rfc { get; set; }
        public string tipoComprobante { get; set; }
        public string condicionPago { get; set; }
        public bool cancelado { get; set; }
        public string fechacancelacion { get; set; }
        public float importe { get; set; }
    }
}
