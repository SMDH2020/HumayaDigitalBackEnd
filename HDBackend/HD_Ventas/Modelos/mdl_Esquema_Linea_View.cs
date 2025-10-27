using HD_Ventas.Modelos.SolicitudesCerradas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos
{
    public class mdl_Esquema_Linea_View
    {
        public IEnumerable<mdl_Modelos_en_Esquema> modelos { get; set; }
        public IEnumerable<mdl_Esquemas_por_Modelo> esquemas { get; set; }

    }
}
