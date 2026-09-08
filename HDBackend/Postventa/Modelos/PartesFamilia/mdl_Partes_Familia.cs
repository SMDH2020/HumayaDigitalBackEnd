using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos.PartesFamilia
{
    public class mdl_Partes_Familia
    {
        public int id { get; set; }
        public string? numero_parte { get; set; }
        public string? linea { get; set; }
        public string? nombre_parte { get; set; }
        public string? familia { get; set; }
        public string? subfamilia_1 { get; set; }
        public string? subfamilia_2 { get; set; }
        public string? usuario { get; set; }
    }
}
