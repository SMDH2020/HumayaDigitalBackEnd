using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Dashboard_Garantias
    {
        public int total_mensajes {  get; set; }
        public int mensajes_pendientes { get; set; }
        public float por_mensajes_pendientes { get; set; }
        public int mensajes_enviados { get; set; }
        public float por_enviados { get; set; }
        public int facturados { get; set; }
        public float por_facturados { get; set; }
        public int mensajes_leidos { get; set; }
        public float por_mensajes_leidos { get; set; }
        public int cliente_con_interes { get; set; }
        public float por_cliente_con_interes { get; set; }
        public int cliente_sin_interes { get; set; }
        public float por_cliente_sin_interes { get; set; }
        public int error { get; set; }
        public float por_error { get; set; }
    }
}
