using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos.Indicadores
{
    public class mdl_Header_Indicadores
    {
        // Mensajes (cuadran con el tablero de mensajeria, solo posventa)
        public int enviados { get; set; }
        public int entregados { get; set; }
        public decimal? pct_entregados { get; set; }
        public int leidos { get; set; }
        public int con_error { get; set; }
        public int numero_erroneo { get; set; }

        // Interacciones (cuadran con el tablero de mensajeria, solo posventa)
        public int interacciones { get; set; }
        public int interesados { get; set; }
        public int no_interesados { get; set; }
        public int respuesta_libre { get; set; }
        public int atendidos { get; set; }
        public int sin_atender { get; set; }

        // Facturacion
        public int folios { get; set; }
        public int facturados_total { get; set; }
        public decimal monto_facturado_total { get; set; }
        public int facturados_con_mensaje { get; set; }
        public decimal monto_facturado_con_mensaje { get; set; }
        public decimal? pct_facturados_con_mensaje { get; set; }
        public decimal? pct_monto_con_mensaje { get; set; }

        // De los leidos, cuantos terminaron facturados
        public int folios_leidos { get; set; }
        public int leidos_facturados { get; set; }
        public decimal monto_leidos_facturados { get; set; }
        public decimal? pct_leidos_facturados { get; set; }
        public decimal? total_posventa { get; set; }
    }
}
