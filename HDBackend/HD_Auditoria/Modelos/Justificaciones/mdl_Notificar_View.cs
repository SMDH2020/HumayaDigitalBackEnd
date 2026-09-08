using HD_Auditoria.Modelos.Programar_Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Justificaciones
{
    public class mdl_Notificar_View
    {
        public mdl_Result_SP estatus { get; set; }

        public IEnumerable<mdl_Notificar_Correo>? correos { get; set; }
    }
}
