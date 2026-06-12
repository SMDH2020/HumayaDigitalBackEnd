using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Conciliacion_Ingresos_Analitica
    {
        public string origen { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public int iddepartamento { get; set; }
        public string departamento { get; set; }
        public string cuenta { get; set; }
        public string v_gl_desc { get; set; }
        public float v_cargos { get; set; }
        public float v_abonos { get; set; }
        public string v_gl_main { get; set; }
        public string v_fecha { get; set; }
        public string v_batch { get; set; }
        public string document_no { get; set; }
        public string serie { get; set; }
        public string folio { get; set; }
        public string fechacancelacion { get; set; }
        public string uuid { get; set; }
        public string estatus { get; set; }
        public string tipoComprobante { get; set; }
        public string rfc { get; set; }
        public string condicionPago { get; set; }
        public string v_desc { get; set; }
        public string v_ref { get; set; }
        public string v_usuario { get; set; }
        public string orig_invoice_no { get; set; }
        public string document_refacturacion { get; set; }
        public string equip { get; set; }
        public int tasa { get; set; }
        public string serie_fiscal { get; set; }
        public string v_relacion { get; set; }
        public string v_fecha_relacion { get; set; }
    }
}
