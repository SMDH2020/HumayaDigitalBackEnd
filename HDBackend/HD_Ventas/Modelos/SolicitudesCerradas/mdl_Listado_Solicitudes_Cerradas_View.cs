using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos.SolicitudesCerradas
{
    public class mdl_Listado_Solicitudes_Cerradas_View
    {
        public IEnumerable<mdl_Solicitudes_Tablero>? tablero { get; set; }
        public IEnumerable<mdl_Solicitudes_Vendedor>? vendedor { get; set; }
        public IEnumerable<mdl_Solicitudes_Sucursal>? sucursal { get; set; }
    }
}
