using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Datos_Carga_Precios_Garantia
    {
        public int idprecio { get; set; }
        public string? modelo { get; set; }
        public double venta_temprana { get; set; }
        public double? venta_tardia { get; set; }
        public double? venta_fin_garantia { get; set; }
        public string? fecha_inicio { get; set; }
        public string? fecha_fin { get; set; }
        public string? tipo_carga { get; set; }

    }
}
