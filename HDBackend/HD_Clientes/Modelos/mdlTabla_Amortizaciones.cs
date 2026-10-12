using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlTabla_Amortizaciones
    {
        public string? folio { get; set; }
        public DateTime inicio { get; set; }
        public double valor_total { get; set; }
        public double importe { get; set; }
        public int plazo { get; set; }
        public double tasa { get; set; }
        public string? usuario { get; set; }
    }
}
