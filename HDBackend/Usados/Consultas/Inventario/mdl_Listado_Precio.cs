using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usados.Consultas.Inventario
{
    public class mdl_Listado_Precio
    {
        public int idinventario { get; set; }
        public double utilidad { get; set; }
        public double margen { get; set; }
        public double precio_lista { get; set; }
        public string? usuario { get; set; }
    }
}
