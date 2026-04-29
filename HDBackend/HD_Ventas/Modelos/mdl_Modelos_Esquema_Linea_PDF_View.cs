using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos
{
    public class mdl_Modelos_Esquema_Linea_PDF_View
    {
        public IEnumerable<mdl_Modelos_en_Esquema> modelos { get; set; }
        public string? esquema { get; set; }
    }
}
