using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlPedido_Detalle_Financiamiento_Info
    {
        public string? folio { get; set; }
        public double precio { get; set; }
        public double descuento { get; set; }
        public double total { get; set; }
        public double deposito { get; set; }
        public double valor_operacion { get; set; }
        public string? tipo_amortizacion { get; set; }
        public string? estatus { get; set; }

    }
}
