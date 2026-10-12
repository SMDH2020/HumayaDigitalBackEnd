using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Justificaciones
{
    public class mdl_Justificaciones_Acciones
    {
        public string? folio { get; set; }
        public int idjust { get; set; }
        public string? motivo { get; set; }
        public string? usuario { get; set; }
        public string? tipo_aceptacion { get; set; }
        public double cantidad_aceptada { get; set; }


    }
}
