using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Dashboard_View
    {
        public string? dashboard_titulo { get; set; }
        public IEnumerable<mdl_Dashboard_Proyecciones>? proyecciones { get; set; }
        public IEnumerable<mdl_Dashboard_Servicio>? servicio { get; set; }
        public IEnumerable<mdl_Dashboard_Refacciones>? refacciones { get; set; }
        public IEnumerable<mdl_Dashboard_Cotizaciones>? cotizaciones { get; set; }
        public IEnumerable<mdl_Dashboard_Vencimiento_Garantias>? vencimiento_garantias_temprana { get; set; }
        public IEnumerable<mdl_Dashboard_Vencimiento_Garantias>? vencimiento_garantias_tardia { get; set; }
        public IEnumerable<mdl_Dashboard_Vencimiento_Garantias>? vencimiento_garantias_checklist { get; set; }
        public IEnumerable<mdl_Dashboard_Grafica_Garantia>? vencimiento_garantias_grafica { get; set; }

        //public string? columnas { get; set; }
    }
}
