using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Info_Cliente_Ubicacion_CRM
    {
        public int orden { get; set; }
        public string direccion { get; set; }
        public bool principal { get; set; }
        public string ubicacion { get; set; }
        public string tipodomicilio { get; set; }
        public string referencia1 { get; set; }
        public string referencia2 { get; set; }
        public bool estatus { get; set; }
        public int idlocalidad { get; set; }
        public string localidad { get; set; }
        public int idmunicipio { get; set; }
        public string municipio { get; set; }
        public int idestado { get; set; }
        public string estado { get; set; }
        public int cp { get; set; }
    }
}
