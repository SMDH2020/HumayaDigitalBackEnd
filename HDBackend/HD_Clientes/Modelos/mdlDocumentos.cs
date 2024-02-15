using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlDocumentos
    {
        public int iddocumento { get; set; }

        public string documento{ get; set; }

        public string tipopersona{ get; set; }

        public string linea_credito{ get; set; }

        public bool Opcional { get; set; }

        public bool Documentacion { get; set; }

        public bool fase2 { get; set; }

        public bool jdf { get; set; }

        public int dias_vigencia { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
