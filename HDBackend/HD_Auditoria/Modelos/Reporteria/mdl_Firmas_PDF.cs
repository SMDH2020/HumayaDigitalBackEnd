using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Reporteria
{
    public class mdl_Firmas_PDF
    {
        public int id_encargado_almacen { get; set; }
        public string? encargado_almacen { get; set; }
        public int id_auditor { get; set; }
        public string? auditor { get; set; }


    }
}
