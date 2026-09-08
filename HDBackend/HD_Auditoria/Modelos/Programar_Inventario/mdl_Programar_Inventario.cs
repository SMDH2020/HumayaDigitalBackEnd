using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Programar_Inventario
{
    public class mdl_Programar_Inventario
    {
        public int id_sucursal { get; set; }
        public string? tipo_inventario { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string fecha_limite_just { get; set; }
        public int id_encargado_alm { get; set; }
        public int id_auditor_ppal { get; set; }
        public string observaciones { get; set; }
        public string auditores_adicionales { get; set; }
        public string categorias { get; set; }
        public string? usuario { get; set; }

    }
}
