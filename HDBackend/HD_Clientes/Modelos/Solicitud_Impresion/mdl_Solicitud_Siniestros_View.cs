using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Solicitud_Impresion
{
    public class mdl_Solicitud_Siniestros_View
    {
        public string? folio { get; set; }
        public string? siniestro { get; set; }
        public double ptotal { get; set; }
        public double pparcial { get; set; }
        public string? indemnizacion { get; set; }
        public double monto { get; set; }
        public string? ciclo { get; set;}
    }
}
