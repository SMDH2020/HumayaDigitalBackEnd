using HD_Auditoria.Modelos.Conteo_Piezas;
using HD_Auditoria.Modelos.Programar_Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Justificaciones
{
    public class mdl_AuditoriaJustificacionesListado_View
    {
        public mdl_Listado_Inventario_Conteo_Header? header { get; set; }
        public IEnumerable<mdl_JustificacionInventario_Listado> Listado { get; set; }
    }
}
