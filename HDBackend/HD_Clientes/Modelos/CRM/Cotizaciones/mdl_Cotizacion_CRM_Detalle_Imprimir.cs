using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Cotizacion_CRM_Detalle_Imprimir
    {
        public int cantidad { get; set; }
        public string modelo { get; set; }          // nombre_servicio
        public string descripcion { get; set; }      // texto plano, puede traer saltos de línea
        public double precio_lista { get; set; }
        public double descuento { get; set; }
        public double impuesto { get; set; }
        public double importe { get; set; }
        public double importe_total { get; set; }
    }
}
