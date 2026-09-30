using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.Tasa_de_intereses
{
    public class Fmdl_TipoTasas
    {
        public int idtipo_tasa { get; set; }
        public string? descripcion { get; set; }
        public DateTime? fecha_inicio { get; set; }
        public DateTime? fecha_fin { get; set; }
        public string? documento { get; set; } = "";
        public string? extension { get; set; } = "";
        public bool estatus { get; set; }
        public string? usuario { get; set; } = "";
    }
}
