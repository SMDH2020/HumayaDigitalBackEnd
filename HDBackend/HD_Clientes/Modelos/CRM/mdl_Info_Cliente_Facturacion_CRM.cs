using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Info_Cliente_Facturacion_CRM
    {
        public int idcliente { get; set; }
        public int cp { get; set; }
        public string domicilio { get; set; }
        public string localidad { get; set; }
        public string municipio { get; set; }
        public string estado { get; set; }
        public string correo { get; set; }
        public int? id_ultimousuario { get; set; }
        public string? ultimousuario { get; set; }
        public DateTime? ultima_fecha { get; set; }
        public string? accion { get; set; }
    }
}
