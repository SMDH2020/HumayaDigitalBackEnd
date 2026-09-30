using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Programar_Inventario
{
    public class mdl_Programar_Inventario_Listado_View
    {
        public IEnumerable<mdl_Listado_Inventario> inventario { get; set; }

        public IEnumerable<mdl_Responsable_Almacen>? responsable { get; set; }
        public IEnumerable<mdl_Categorias>? categorias { get; set; }

        public IEnumerable<mdl_Sucursales>? sucursales { get; set; }

        public IEnumerable<mdl_Usuarios>? auditores { get; set; }

        public IEnumerable<mdl_Usuarios>? empleados { get; set; }

    }
}
