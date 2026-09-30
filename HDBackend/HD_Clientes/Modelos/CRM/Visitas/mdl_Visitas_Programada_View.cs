using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Visitas
{
    public class mdl_Visitas_Programada_View
    {
        public IEnumerable<mdl_Listado_Visitas_Programadas> listado_visitas { get; set; }
        public mdl_Header_Info_Visitas_Programadas header_info { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_tipo_visita { get; set; }
        public IEnumerable<mdl_Opciones_Lineas_CRM> opciones_lineas { get; set; }
        public mdl_Permisos_CRM permisos { get; set; }

    }
}