using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Programar_Inventario
{
    public class mdl_ProgramarPasillos_View
    {
        public IEnumerable<mdl_Usuarios> auditores { get; set; }

        public IEnumerable<mdl_Pasillos>? pasillos { get; set; }

        public IEnumerable<mdl_Pasillos>? asignaciones { get; set; }
    }
}
