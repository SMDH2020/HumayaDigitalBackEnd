using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Listado_Incidencias_Candidatos_Refacturacion
    {
        public int idsucursal {  get; set; }
        public string sucursal {  get; set; }
        public int idcliente { get; set; }
        public string razonsocial { get; set; }
        public string document_no { get; set; }
        public string serie { get; set; }
        public string folio { get; set; }
        public float importe_venta {  get; set; }
        public string von_no { get; set; }
        public string document_reversa { get; set; }
        public string document_refacturacion { get; set; }
    }
}
