using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Solicitud_Impresion
{
    public class mdl_Solicitud_Domicilios_View
    {
        public string? folio { get; set; }
        public string? municipio { get; set; }
        public string? estado { get; set; }
        public string? cp { get; set; }
        public string? localidad { get; set; }
        public string? direccion { get; set; }
        public string? tipodomicilio { get; set; }
    }
}
