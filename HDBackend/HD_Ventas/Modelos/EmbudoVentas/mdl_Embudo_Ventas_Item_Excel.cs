using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos.EmbudoVentas
{
    public class mdl_Embudo_Ventas_Item_Excel
    {
        public string Fase { get; set; }
        public string Linea { get; set; }
        public string Columna { get; set; }
        public int Cantidad { get; set; }
        public decimal Monto { get; set; }
    }
}
