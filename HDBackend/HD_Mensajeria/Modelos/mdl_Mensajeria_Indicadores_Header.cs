using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Modelos
{
    public class mdl_Mensajeria_Indicadores_Header
    {
        public int enviados { get; set; }
        public int entregados { get; set; }

        public int leidos { get; set; }
        public int interesados { get; set; }
        public int atendidos { get; set; }
        public int sin_atender { get; set; }
        public int con_error { get; set; }
        public int numero_erroneo { get; set; }
    }
}
