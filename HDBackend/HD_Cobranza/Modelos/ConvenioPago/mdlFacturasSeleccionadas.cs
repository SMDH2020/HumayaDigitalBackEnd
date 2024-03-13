using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.ConvenioPago
{
    public class mdlFacturasSeleccionadas
    {
        public int id { get; set; }
        public int idcliente { get; set; }
        public string? documento { get; set; }
        public string? descripcion { get; set; }
        public int diasvencido { get; set; }
        public string? fecha { get; set; }
        public double importefactura { get; set; }
        public double importepagado { get; set; }
        public double intereses { get; set; }
        public double saldo { get; set; }
        public string? vencimiento { get; set; }
        public double total => saldo + intereses;
    }
}
