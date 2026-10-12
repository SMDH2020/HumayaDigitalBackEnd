using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.CondonacionIntereses
{
    public class mdl_Validar_Condonaciones_Clientes
    {
        public int total_condonaciones { get; set; }
        public bool tiene_pendiente { get; set; }
        public string? ultimo_folio { get; set; }
        public string? ultimo_estatus { get; set; }
        public int? dias_desde_autorizacion { get; set; }
    }
}
