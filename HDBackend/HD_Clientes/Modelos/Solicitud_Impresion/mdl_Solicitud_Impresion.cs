using HD.Clientes.Modelos.Pedido_Impresion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Solicitud_Impresion
{
    public class mdl_Solicitud_Impresion
    {
        public mdl_Solicitud_Datos_Generales_View? datosgenerales { get; set; }
        public List<mdl_Solicitud_Contactos_View>? contactos{ get; set; }
        public List<mdl_Solicitud_Domicilios_View>? domicilios{ get; set; }
        public List<mdl_Solicitud_Cultivos_View>? cultivos { get; set; }
        public List<mdl_Solicitud_Balance_Patrimonial_View>? balancepatrimonial{ get; set; }
        public List<mdl_Solicitud_Estado_Resultados_View>? estadoresultados{ get; set; }
        public List<mdl_Solicitud_Otros_Ingresos_View>? otrosingresos{ get; set; }
        public List<mdl_Solicitud_Siniestros_View>? siniestros{ get; set; }
    }
}
