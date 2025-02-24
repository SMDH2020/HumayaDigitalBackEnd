using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductoAliado.Modelos.Inventario
{
    public class mdl_promocion_Producto_Aliado
    {
        public int idpromocion { get; set; }
        public int idinventario { get; set; }
        public string? descripcion { get; set; }
        public string? vigencia { get; set; }
        public string? usuario { get; set; }
    }
}
