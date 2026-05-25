using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Justificaciones
{
    public class mdl_mensajes_Timeline
    {
        public int idj_ust { get; set; }
        public int id_conteo { get; set; }
        public string? tipo_actor { get; set; }
        public int id_usuario { get; set; }
        public string? usuario { get; set; }
        public string? comentario_usuario { get; set; }
        public string? estatus { get; set; }
        public string? fecha_envio { get; set; }
        public string? fecha_revision { get; set; }
        public int id_auditor { get; set; }
        public string? auditor { get; set; }
        public string? motivo_rechazo { get; set; }

    }
}
