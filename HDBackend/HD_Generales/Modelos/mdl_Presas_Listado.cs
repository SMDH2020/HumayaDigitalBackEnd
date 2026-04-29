using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Generales.Modelos
{
    public class mdl_Presas_Listado
    {
        public string? idpresa { get; set; }
        public string? presa { get; set; }
        public string? nombre_corto { get; set; }
        public int idestado { get; set; }
        public string? municipio { get; set; }
        public string? fecha { get; set; }
        public double porcentaje { get; set; }

    }
}
