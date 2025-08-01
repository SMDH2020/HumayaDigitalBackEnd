using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Dashboard_Cotizaciones
    {
        public string? orden { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public int idcliente { get; set; }
        public string? cliente { get; set; }
        public string? vendedor { get; set; }
        public string? fecha { get; set; }
        public double total { get; set; }
        public string? whatsapp { get; set; }
        public string? estado { get; set; }
        public string? mensaje1 { get; set; }
        public string? mensaje2 { get; set; }

    }
}
