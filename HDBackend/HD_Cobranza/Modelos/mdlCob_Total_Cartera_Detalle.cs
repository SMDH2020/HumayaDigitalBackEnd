using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos
{
    public class mdlCob_Total_Cartera_Detalle
    {
        public int id { get; set; }
        public string? estatus { get; set; }
        public string? linea { get; set; }
        public string? sucursal { get; set; }
        public int idcliente { get; set; }
        public string? razonsocial { get; set; }
        public string? documento { get; set; }
        public int diasvencido { get; set; }
        public string? vencimiento { get; set; }
        public double interesdiariobase { get; set; }
        public double saldo { get; set; }
        public double interesbase { get; set; }
        public double importe { get; set; }
    }
}
