using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Filtro_Avanzado_Clientes_View
    {
        public IEnumerable<mdl_Opciones_Estado_CRM> opciones_estado { get; set; }
        public IEnumerable<mdl_Opciones_Municipio_CRM> opciones_municipio { get; set; }
    }
}
