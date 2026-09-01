using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Justificaciones
{
    public class mdl_Evidencias_Timeline
    {
        public int id_evidencia { get; set; }
        public int id_just { get; set; }
        public string? nombre_archivo { get; set; }
        public string? tipo_archivo { get; set; }
        public string? ruta_servidor { get; set; }
        public int tamanio_bytes { get; set; }
    }
}
