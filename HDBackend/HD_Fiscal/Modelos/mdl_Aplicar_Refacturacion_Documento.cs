using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Aplicar_Refacturacion_Documento
    {
        public int document_candidato { get; set; }
        public int document_refacturacion { get; set; }
        public int document_reversa { get; set; }
    }
}
