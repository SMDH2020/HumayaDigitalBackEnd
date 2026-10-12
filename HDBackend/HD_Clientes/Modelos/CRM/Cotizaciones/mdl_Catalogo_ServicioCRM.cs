using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Catalogo_ServicioCRM
    {
        public int IdServicio { get; set; } 
        public int IdLineaVenta { get; set; }
        public string NombreServicio { get; set; }
        public string Descripcion { get; set; }
        public double PrecioLista { get; set; }
        public double Descuento { get; set; }
        public double Impuesto { get; set; }
        public int Usuario { get; set; }
    }
}
