using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_orden_Cotizacion
    {
        public int columna { get; set; }

        public int folio { get; set; }
        public string descripcion { get; set; }
        public int disponible { get; set; }
        public int cantidad { get; set; }
        public float precio { get; set; }
        public float subtotal { get; set; }
        public float descuento { get; set; }
        public float total { get; set; }
    }
}
