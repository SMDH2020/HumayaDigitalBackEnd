using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Comentarios_Timeline_Mensajeria_Guardar
    {
        public int idcomentario { get; set; }
        public string? tipo { get; set; }
        public string? tipo_contacto { get; set; }
        public string? fase { get; set; }
        public string? recordatorio { get; set; }
        public string? comentario { get; set; }
        public double horometro { get; set; }
        public string? usuario { get; set; }
        public string? documento_factura { get; set; }

    }
}
