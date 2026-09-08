using HD_Auditoria.Modelos.Programar_Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Justificaciones
{
    public class mdl_TimelineView
    {
        public IEnumerable<mdl_mensajes_Timeline> mensajes { get; set; }

        public IEnumerable<mdl_Evidencias_Timeline>? evidencia { get; set; }
    }
}
