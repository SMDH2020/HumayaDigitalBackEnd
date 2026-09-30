using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Modelos
{
    public class mdl_LineasScorecard
    {
        public int idlinea { get; set; }
        public string? descripcion { get; set; } = "";
        public bool estatus { get; set; }
        public string? usuario { get; set; } = "";
    }
}
