using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.ConvenioPago
{
    public class mdlListadoConveniosVista
    {
        public string? folio { get; set; }

        public int  monto{ get; set; }

        public DateTime fecha_convenio { get; set; }

        public string? mediocontacto { get; set; }

        public int createuser { get; set; }

        public string? NombreCompleto { get; set; }

    }
}
