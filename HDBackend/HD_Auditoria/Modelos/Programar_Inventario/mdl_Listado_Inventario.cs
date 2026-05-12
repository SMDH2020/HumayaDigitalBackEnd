using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Programar_Inventario
{
    public class mdl_Listado_Inventario
    {
        public string? folio { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public string? fecha_inicio { get; set; }

        public int anio { get; set; }
        public int mes { get; set; }
        public int dia { get; set; }

        public string? estatus { get; set; }
    }
}
