using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos.Indicadores
{
    public class mdl_Indicadores_Detalle
    {
        public string Cliente { get; set; }
        public string Telefono { get; set; }
        public int? idresponsable { get; set; }
        public string Responsable { get; set; }
        public int? IDSucursal { get; set; }
        public string Sucursal { get; set; }
        public int FacturadosTotal { get; set; }
        public decimal MontoTotal { get; set; }
        public int MensajesEnviados { get; set; }
        public int FacturadosMensajeria { get; set; }
        public decimal MontoMensajeria { get; set; }
    }
}
