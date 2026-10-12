using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.Estado_Resultados
{
    public class Fmdl_EstadoResultados_Data
    {
        public string? depto { get; set; }
        public List<Fmdl_EstadoResultados_View> data { get; set; }
    }
}
