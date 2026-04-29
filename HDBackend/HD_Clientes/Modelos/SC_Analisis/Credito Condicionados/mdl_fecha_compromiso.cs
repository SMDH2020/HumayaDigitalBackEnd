using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados
{
    public class mdl_fecha_compromiso
    {
        public string? folio { get; set; }
        public string? usuario { get; set; }
        public DateTime fecha_compromiso { get; set; }
        public string? comentarios { get; set; }
        public string? habilitar_fecha { get; set; }
    }
}
