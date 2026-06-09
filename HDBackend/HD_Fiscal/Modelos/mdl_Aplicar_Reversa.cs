using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Aplicar_Reversa
    {
        public int document_cancelacion { get; set; }
        public int document_orig { get; set; }
        public int document_refacturacion { get; set; }
    }
}
