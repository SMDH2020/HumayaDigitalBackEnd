using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados
{
    public class mdl_fecha_compromiso_documentos
    {
        public string? folio { get; set; }
        public string? usuario { get; set; }
        public string? comentarios { get; set; }
        public DateTime fecha_compromiso { get; set; }
        public bool enviar_revision { get; set; }
        public int iddocumento { get; set; }
        public string documento { get; set; }
        public bool tiene_documentacion { get; set; }

    }
}
