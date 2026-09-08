using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Reporteria.ImagentoPDF
{
    public class mdl_Covertor
    {
        public string? folio { get; set; }
        public int iddocumento { get; set; }
        public string? nombreDocumento { get; set; }

        public string? extension { get; set; }
        public string? vigencia { get; set; }
        public string? comentarios { get; set; }
        public string? usuario { get; set; }


        public List<string> ImagenesBase64 { get; set; }

    }
}
