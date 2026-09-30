using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Listado_Incidencias_Reversas_Pendientes_Aplicar
    {
        public string document_no {  get; set; }
        public string invo_date { get; set; }
        public float total { get; set; }
        public float gst {  get; set; }
        public string batch { get; set; }
        public string series_code { get; set; }
        public string fiscal_document_no { get; set; }
        public string descripcion { get; set; }
        public string orig_invoice_no { get; set; }
        public string document_refacturacion { get; set; }
    }
}
