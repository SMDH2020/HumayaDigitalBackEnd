using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Programar_Inventario
{
    public class mdl_Result_SP
    {
        public string? folio { get; set; }
        public int resultado { get; set; }
        public int completado { get; set; }
        public int rechazado { get; set; }
        public int finalizado { get; set; }
        public string? mensaje { get; set; }

    }
}
