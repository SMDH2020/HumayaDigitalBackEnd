using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Justificaciones
{
    public class mdl_Justificaciones_Guardar
    {
        public string? folio { get; set; }
        public int idconteo { get; set; }
        public int idjust { get; set; }
        public string? comentario { get; set; }
        public List<IFormFile>? archivos { get; set; }
        public string? usuario { get; set; }

    }
}
