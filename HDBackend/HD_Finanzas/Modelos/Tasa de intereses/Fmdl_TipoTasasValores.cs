using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.Tasa_de_intereses
{
    public class Fmdl_TipoTasasValores
    {
        public int idtipo_tasa_valor { get; set; }
        public int idtipo_tasa { get; set; }
        public int plazo { get; set; }
        public double porcentaje { get; set; }
        public bool estatus { get; set; }
        public string? usuario { get; set; } = "";
    }
}
