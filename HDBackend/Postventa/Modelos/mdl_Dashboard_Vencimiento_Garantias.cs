using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Dashboard_Vencimiento_Garantias
    {
        public string modelo { get; set; }
        public string num_serie { get; set; }
        public string expiracion_format { get; set; }
        public float monto { get; set; }
        public int IDSucursal { get; set; }
        public string sucursal { get; set; }
        public string cliente { get; set; }
        public string contrato_adquirido { get; set; }
        public string grupo {  get; set; }
    }
}
