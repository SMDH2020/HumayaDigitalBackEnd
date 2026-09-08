using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductoAliado.Modelos.Inventario
{
    public class mdl_Listado_Precio_Producto_Aliado
    {
        public int idinventario { get; set; }
        public double utilidad { get; set; }
        public double margen { get; set; }
        public double precio_lista { get; set; }
        public string? usuario { get; set; }
    }
}
