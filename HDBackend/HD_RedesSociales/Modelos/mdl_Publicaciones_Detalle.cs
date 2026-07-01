using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_RedesSociales.Modelos
{
    public class mdl_Publicaciones_Detalle
    {
        public string? Folio { get; set; }
        public int? Consecutivo { get; set; }
        public DateTime? Fecha_Envio { get; set; }
        public string? Estatus { get; set; }
    }
}