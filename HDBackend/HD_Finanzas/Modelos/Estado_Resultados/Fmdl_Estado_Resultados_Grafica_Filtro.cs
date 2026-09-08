using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.Estado_Resultados
{
    public class Fmdl_Estado_Resultados_Grafica_Filtro
    {
        public int ejercicio {  get; set; }
        public string periodo { get; set; }
        public int ejerciciofin {  get; set; }
        public string periodofin { get; set; }
        public string? adr { get; set; }
        public string? sucursales { get; set; }
        public string? departamentos { get; set; }
    }
}
