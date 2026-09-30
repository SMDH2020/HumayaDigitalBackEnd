using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Cargar_Precios_Garantia
    {
        public string? tipo_carga { get; set; }
        public string? inicio_vigencia { get; set; }
        public string? vigencia { get; set; }

        public IEnumerable<mdl_Datos_Carga_Precios_Garantia>? datos { get; set; }

    }
}
