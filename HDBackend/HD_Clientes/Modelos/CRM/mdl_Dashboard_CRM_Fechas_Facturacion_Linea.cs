using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Dashboard_CRM_Fechas_Facturacion_Linea
    {
        public int idcliente { get; set; }
        public int idlinea { get; set; }
        public string? linea { get; set; }
        public string? ultima_fecha_facturacion { get; set; }

    }
}
