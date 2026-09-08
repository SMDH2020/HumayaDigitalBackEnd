using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.Juridico
{
    public class mdl_Clientes_Juridico_Guardar
    {
        public int idcliente { get; set; }
        public string? estatus { get; set; }
        public string? comentarios { get; set; }
        public string? detalle { get; set; } = "";
        public string? usuario { get; set; }

    }
}
