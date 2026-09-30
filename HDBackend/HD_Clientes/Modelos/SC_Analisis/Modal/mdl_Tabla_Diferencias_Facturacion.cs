using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.Modal
{
    public class mdl_Tabla_Diferencias_Facturacion
    {
        public int documento_hd { get; set; }
        public int idequip_hd { get; set; }
        public string? vencimiento_hd { get; set; }
        public double? tasas_hd { get; set; }
        public int documento_facturacion { get; set; }
        public int idequip_facturacion { get; set; }
        public string? vencimiento_facturacion { get; set; }
        public double? tasas_facturacion { get; set; }

    }
}
