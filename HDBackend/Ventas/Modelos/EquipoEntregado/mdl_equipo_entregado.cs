using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Modelos.EquipoEntregado
{
    public class mdl_equipo_entregado
    {
        public bool entregado { get; set; }
        public int? id_CEE { get; set; }
        public string folio_cotizacion { get; set; }
        public int orden_cotizacion { get; set; }
        public int idcliente { get; set; }
        public string comentarios { get; set; }
        public DateTime? fecha_entrega { get; set; }
        public string folio_solicitud { get; set; }
        public int? createuser { get; set; }
        public DateTime? createdate { get; set; }
    }
}
