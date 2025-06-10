using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.ProyeccionesGastos
{
    public class mdl_Filtro_Proyecciones_Gastos
    {
        public int ejercicio { get; set; }
        public int ejercicioant { get; set; }
        public string comparar { get; set; }
        public string periodo { get; set; }
        public string adr { get; set; }
        public string departamento { get; set; }
        public string sucursal { get; set; }
    }
}
