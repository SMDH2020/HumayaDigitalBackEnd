using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.Juridico
{
    public class mdl_Clientes_Juridico_Correo
    {
        public int idcliente { get; set; }
        public int idusuario { get; set; }
        public string? nombre { get; set; }
        public string? nombre_cliente { get; set; }
        public string? correo { get; set; }
        public string? estatus { get; set; }
        public string? comentarios { get; set; }
    }
}
