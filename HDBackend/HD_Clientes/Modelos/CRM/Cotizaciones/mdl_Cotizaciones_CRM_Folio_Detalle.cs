using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Cotizaciones_CRM_Folio_Detalle
    {
        public string folio { get; set; }
        public int id_servicio { get; set; }
        public string nombre_servicio { get; set; }
        public string descripcion { get; set; }
        public double precio_lista { get; set; }
        public int cantidad { get; set; }
        public double importe { get; set; }
        public double descuento { get; set; }
        public double impuesto { get; set; }
        public double importe_total { get; set; }
    }
}
