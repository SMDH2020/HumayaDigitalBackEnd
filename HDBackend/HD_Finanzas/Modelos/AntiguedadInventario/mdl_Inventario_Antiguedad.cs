using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.AntiguedadInventario
{
    public class mdl_Inventario_Antiguedad
    {
        public int idadr { get; set; }
        public string adr { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public string familia { get; set; }
        public string descfamilia { get; set; }
        public string modelo { get; set; }
        public string nombremodelo { get; set; }
        public string neconomico { get; set; }
        public DateTime fecharecepcion { get; set; }
        public int antiguedaddias { get; set; }
        public string antiguedadmes { get; set; }
        public int nummonth { get; set; }
        public double costo { get; set; }
        public string rd { get; set; }
        public string nip { get; set; }
        public string invstatus { get; set; }
    }
}
