using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Reportes
{
    public class mdl_Reporte_GeolocalizacionCRM
    {
        public int idcliente { get; set; }
        public string razon_social { get; set; }
        public int idvendedor { get; set; }
        public string vendedor { get; set; }
        public int? IDSucursal { get; set; }
        public string sucursal { get; set; }
        public int geolocalizacion { get; set; }
        public string ubicacion { get; set; }
        public int? idlocalidad { get; set; }
        public string localidad { get; set; }
        public int? idmunicipio { get; set; }
        public string municipio { get; set; }
        public int? idestado { get; set; }
        public string estado { get; set; }
    }
}
