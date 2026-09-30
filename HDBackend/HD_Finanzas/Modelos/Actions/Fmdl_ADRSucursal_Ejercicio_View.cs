using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.Actions
{
    public class Fmdl_ADRSucursal_Ejercicio_View
    {
        public IEnumerable<FmdlADRScucursalDep>? filtro { get; set; }

        public IEnumerable<Fmdl_Ejercicios_Conciliaciones>? fechas { get; set; }
    }
}
