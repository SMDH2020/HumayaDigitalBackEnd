using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos.Indicadores
{
    public class mdl_Indicadores_Historico
    {
        public int Ejercicio { get; set; }
        public int Periodo { get; set; }
        public int MensajesEnviados { get; set; }
        public int FacturadosTotal { get; set; }
        public decimal MontoTotal { get; set; }
        public int FacturadosMensajeria { get; set; }
        public decimal MontoMensajeria { get; set; }
    }
}
