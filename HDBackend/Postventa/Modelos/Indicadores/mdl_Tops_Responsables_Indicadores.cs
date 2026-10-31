using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos.Indicadores
{
    public class mdl_Tops_Responsables_Indicadores
    {
        public int? idresponsable { get; set; }                    // NULL si el vendedor no hizo match con Empleados
        public string Responsable { get; set; } = string.Empty;
        public int mensajes_validos { get; set; }
        public int con_respuesta { get; set; }
        public int facturados { get; set; }
        public decimal monto_facturado { get; set; }
        public decimal pct_facturacion { get; set; }
    }
}
