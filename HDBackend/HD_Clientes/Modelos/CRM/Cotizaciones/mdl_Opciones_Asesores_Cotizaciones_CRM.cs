using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Opciones_Asesores_Cotizaciones_CRM
    {
        public int IDEmpleado { get; set; }
        public string empleado { get; set; }
        public int IDSucursal { get; set; }
        public string sucursal { get; set; }
        public string gerente { get; set; }
    }
}
