using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Modelos.CotizacionesVentas
{
    public class mdl_Listado_Cotizaciones_Nuevo
    {
        public string folio { get; set; }
        public int asesorventa { get; set; }
        public string nombre_asesor { get; set; }
        public int idcliente { get; set; }
        public string razon_social { get; set; }
        public string asunto { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public string linea { get; set; }
        public string modelo { get; set; }
        public int idpromocion {  get; set; }
        public string esquema { get; set; }
        public float monto_total { get; set; }
        public string fase_cotizacion { get; set; }
        public string fecha_venta { get; set; }
        public string vigencia { get; set; }
        public string createdate { get; set; }
        public int createuser { get; set; }
    }
}
