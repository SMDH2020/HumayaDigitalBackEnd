using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Modelos.EquipoEntregado
{
    public class mdl_equipo_entregado_view
    {
        public mdl_equipo_entregado entrega { get; set; }
        public mdl_Solicitud solicitud { get; set; }
        public List<mdl_Solicitud> solicitudes_cliente { get; set; }
    }
}
