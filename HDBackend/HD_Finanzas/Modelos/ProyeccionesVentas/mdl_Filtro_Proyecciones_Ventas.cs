using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.ProyeccionesVentas
{
    public class mdl_Filtro_Proyecciones_Ventas
    {
        public int ejercicio { get; set; }
        public int ejercicioant { get; set; }
        public string escenario {  get; set; }
        public string comparar { get; set; }
        public string periodo { get; set; }
        public string adr { get; set; }
        public string departamento { get; set; }
        public string sucursal { get; set; }

        //    public string titulo1 { get; set; }
        //    public string titulo2 { get; set; }

        //    public string displayadr { get; set; }
        //    public string displaydepartamento { get; set; }
        //    public string displaysucursal { get; set; }
    }
}
