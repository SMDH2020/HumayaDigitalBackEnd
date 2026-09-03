using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Parque_Maquinaria
{
    public class mdl_Listado_Parque_MaquinariaCRM
    {
        public int idcliente { get; set; }
        public string razon_social { get; set; }
        public string categoria { get; set; }
        public string tipo { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string serie { get; set; }
        public int? anio { get; set; }
        public string comentarios { get; set; }
    }
}
