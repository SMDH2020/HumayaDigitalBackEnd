using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Modelos.EquipoEntregado
{
    public class mdl_Guardar_Equipo_Entregado
    {
        public string folio_cotizacion { get; set; }
        public int orden_cotizacion { get; set; }
        public string comentarios { get; set; }
        public DateTime fecha_entrega { get; set; }
        public string? folio_solicitud { get; set; }
        public int usuario { get; set; }
    }
}
