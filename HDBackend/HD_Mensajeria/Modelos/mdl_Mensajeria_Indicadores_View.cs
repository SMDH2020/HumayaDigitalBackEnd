using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Modelos
{
    public class mdl_Mensajeria_Indicadores_View
    {

        public mdl_Mensajeria_Indicadores_Header header { get; set; }
        public List<mdl_Mensajeria_Indicadores_Top> masRespuestas { get; set; }
        public List<mdl_Mensajeria_Indicadores_Top> menosRespuestas { get; set; }
        public List<mdl_Mensajeria_Indicadores_Detalle> listado { get; set; }


    }
}
