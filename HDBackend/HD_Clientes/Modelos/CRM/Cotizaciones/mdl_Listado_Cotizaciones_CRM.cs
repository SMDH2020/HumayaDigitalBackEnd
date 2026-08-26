using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Listado_Cotizaciones_CRM
    {
        public string folio { get; set; }
        public int idcliente { get; set; }
        public string razon_social { get; set; }
        public int id_propietario { get; set; }
        public string propietario { get; set; }
        public string asunto { get; set; }
        public string nombre_contacto { get; set; }
        public DateTime vigencia { get; set; }
        public int id_asesor { get; set; }
        public string asesor { get; set; }
        public int id_sucursal { get; set; }
        public string sucursal { get; set; }
        public int id_gerente { get; set; }
        public string gerente { get; set; }
        public int id_origen { get; set; }
        public string origen { get; set; }
        public int id_tipo_pago { get; set; }
        public string tipo_pago { get; set; }
        public double subtotal { get; set; }
        public double descuento { get; set; }
        public double ajuste { get; set; }
        public double total { get; set; }
        public int createuser { get; set; }
        public string creador { get; set; }
        public DateTime createdate { get; set; }
    }
}
