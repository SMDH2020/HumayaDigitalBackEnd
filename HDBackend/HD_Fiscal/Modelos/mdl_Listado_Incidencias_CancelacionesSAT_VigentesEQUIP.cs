using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Listado_Incidencias_CancelacionesSAT_VigentesEQUIP
    {
        public int idregistro {  get; set; }
        public string invo_type { get; set; }
        public string document_no { get; set; }
        public string cust_ord_no { get; set; }
        public string invo_date { get; set; }
        public int ro_number { get; set; }
        public string special_inst { get; set; }
        public string series_code { get; set; }
        public string fiscal_document_no { get; set; }
        public string batch { get; set; }
        public string uuid { get; set; }
        public string rfc { get; set; }
        public string tipoComprobante { get; set; }
        public string condicionPago { get; set; }
        public bool cancelado { get; set; }
        public string fechaCancelacion { get; set; }
        public float total { get; set; }
        public string batch_cancelacion { get; set; }
    }
}
