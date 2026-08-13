using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Info_Cliente_CRM
    {
        public int idcliente { get; set; }
        public string rfc { get; set; }
        public string razon_social { get; set; }
        public string tipo_persona { get; set; }
        public string? principal { get; set; }
        public string? estatus_cliente { get; set; }
        public string? origen_cliente { get; set; }
        public string? tipo_cliente { get; set; }
        public int idvendedor { get; set; }
        public string? clasificacion { get; set; }
        public string? nivel_tecnologia { get; set; }
        public string? etiqueta { get; set; }
        public string? telefono { get; set; }
        public string? correo { get; set; }
        public string? nombre_completo { get; set; }
        public string? zona { get; set; }
        public string? zona_nombre { get; set; }
        public int? id_ultimousuario { get; set; }
        public string? ultimousuario { get; set; }
        public DateTime? ultima_fecha { get; set; }
        public string? accion { get; set; }
    }
}
