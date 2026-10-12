using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.RelacionDocumentosSC
{
    public class mdl_RelacionDocumentosSC_Listado_View
    {
        public IEnumerable<mdl_RelacionDocumentosSC_Listado> Listado { get; set; }
        public IEnumerable<mdl_Documentos_Listado> DocumentosMhusa { get; set; }
        public IEnumerable<mdl_Documentos_Listado> DocumentosJDF { get; set; }


    }
}
