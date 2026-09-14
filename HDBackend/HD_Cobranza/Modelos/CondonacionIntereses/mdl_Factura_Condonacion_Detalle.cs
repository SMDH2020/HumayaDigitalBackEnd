using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.CondonacionIntereses
{
    public class mdl_Factura_Condonacion_Detalle
    {
        public int id { get; set; }
        public string Folio { get; set; }
        public string Documento { get; set; }
        public string? Serie { get; set; }
        public double Saldo { get; set; }
        public double Inormal { get; set; }
        public double Imoratorio { get; set; }
    }
}
