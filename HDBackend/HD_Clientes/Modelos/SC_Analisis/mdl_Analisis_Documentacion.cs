using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis
{
    public class mdl_Analisis_Documentacion
    {
        public string? folio { get; set; }
        public int idproceso { get; set; }
        public int iddocumento { get; set; }
        public int consecutivo { get; set; }
        public string? comentarios { get; set; }
        public string? estatus { get; set; }
        public string? usuario { get; set; }
        public string? vencimiento { get; set; }
    }
}
