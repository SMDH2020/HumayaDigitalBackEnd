using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos.Indicadores
{
    public class mdl_Header_Indicadores
    {
        public int total_registros { get; set; }
        public int mensajes_enviados { get; set; }
        public int facturados_total { get; set; }
        public decimal monto_facturado_total { get; set; }
        public int mensajes_con_error { get; set; }
        public int mensajes_entregados { get; set; }
        public int mensajes_leidos { get; set; }
        public int facturados_con_mensaje { get; set; }
        public decimal? pct_facturados_con_mensaje { get; set; }   // NULL si facturados_total = 0

        // Complementarios
        public int sin_mensaje { get; set; }
        public int con_respuesta { get; set; }

        // Tarjeta de interacciones
        public int interacciones { get; set; }
        public int interesados { get; set; }
        public int no_interesados { get; set; }
        public int respuesta_libre { get; set; }
        public int atendidos { get; set; }
        public int sin_atender { get; set; }
        public decimal monto_facturado { get; set; }
        public decimal ticket_promedio { get; set; }
        public int mensajes_sin_responsable { get; set; }
    }
}
