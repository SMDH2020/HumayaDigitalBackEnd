using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Listado_Catalogo_ServicioCRM
    {
        public int id_servicio { get; set; }
        public int id_linea_venta { get; set; }
        public string linea_venta { get; set; }
        public string nombre_servicio { get; set; }
        public string descripcion { get; set; }
        public double precio_lista { get; set; }
        public double descuento { get; set; }
        public double impuesto { get; set; }
        public bool estatus { get; set; }
    }
}
