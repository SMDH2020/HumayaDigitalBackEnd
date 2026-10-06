using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos.Indicadores
{
    public class mdl_Indicadores_Postventa_View
    {
        public mdl_Header_Indicadores Header { get; set; } = new();
        //public List<mdl_Tops_Responsables_Indicadores> TopMasFacturan { get; set; } = new();
        //public List<mdl_Tops_Responsables_Indicadores> TopMenosFacturan { get; set; } = new();
        public List<mdl_Desglose_Indicadores> Desglose { get; set; } = new();
        public List<mdl_Indicadores_Detalle> Detalle { get; set; } = new();
        public List<mdl_Indicadores_Historico> Historico { get; set; } = new();

    }
}
