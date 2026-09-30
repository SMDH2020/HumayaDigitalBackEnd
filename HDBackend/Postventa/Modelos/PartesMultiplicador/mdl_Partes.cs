using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos.PartesMultiplicador
{
    public class mdl_Partes
    {
        public int idparte { get; set; }
        public string? parte { get; set; }
        public double multiplicador { get; set; }
        public string? usuario { get; set; }

    }
}
