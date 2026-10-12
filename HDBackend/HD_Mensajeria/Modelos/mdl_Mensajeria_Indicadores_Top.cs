using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Modelos
{
    public class mdl_Mensajeria_Indicadores_Top
    {
        public int idresponsable { get; set; }
        public string Responsable { get; set; }
        public int recibidos { get; set; }
        public int enviados { get; set; }
        public int pct_respuesta { get; set; }
    }
}
