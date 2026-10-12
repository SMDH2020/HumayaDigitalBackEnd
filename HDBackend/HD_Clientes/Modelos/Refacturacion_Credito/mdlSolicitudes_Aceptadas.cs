using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Refacturacion_Credito
{
    public class mdlSolicitudes_Aceptadas
    {
        public string? folio { get; set; }
        public string? sucursal { get; set; }
        public string? cliente { get; set; }
        public string? tipo_credito { get; set; }
        public string? vendedor { get; set; }
        public string? fecha { get; set; }
    }
}
