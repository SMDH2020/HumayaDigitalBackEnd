using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usados.Consultas.Inventario
{
    public class mdl_promocion
    {
        public int idpromocion { get; set; }
        public int idinventario { get; set; }
        public string? descripcion { get; set; }
        public string? vigencia { get; set; }
        public string? usuario { get; set; }
    }
}
