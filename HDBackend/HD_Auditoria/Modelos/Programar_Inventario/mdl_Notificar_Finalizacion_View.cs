using HD_Auditoria.Modelos.Justificaciones;
using HD_Auditoria.Modelos.Reporteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Programar_Inventario
{
    public class mdl_Notificar_Finalizacion_View
    {
        public mdl_Result_SP estatus { get; set; }
        public IEnumerable<mdl_Notificar_Correo>? correos { get; set; }
        public IEnumerable<mdl_Finalizacion_Diferencias> diferencias { get; set; }

        public mdl_Finalizacion_Metricas info { get; set; }

        public mdl_Firmas_PDF firmas { get; set; }


    }
}
