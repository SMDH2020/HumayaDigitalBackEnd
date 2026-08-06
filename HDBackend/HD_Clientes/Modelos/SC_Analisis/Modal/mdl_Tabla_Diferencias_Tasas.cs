using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.Modal
{
    public class mdl_Tabla_Diferencias_Tasas
    {
        public int documento_hd { get; set; }
        public string? folio_hd { get; set; }
        public string? vencimiento_hd { get; set; }
        public int documento_tasas { get; set; }
        public string? folio_tasas { get; set; }
        public string? vencimiento_tasas { get; set; }

    }
}
