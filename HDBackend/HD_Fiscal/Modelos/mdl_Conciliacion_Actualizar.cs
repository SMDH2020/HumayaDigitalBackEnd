using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Conciliacion_Actualizar
    {
        public int ejercicio { get; set; }
        public int periodo { get; set; }
        public string? comentario { get; set; }
        public int usuario { get; set; }
    }
}
