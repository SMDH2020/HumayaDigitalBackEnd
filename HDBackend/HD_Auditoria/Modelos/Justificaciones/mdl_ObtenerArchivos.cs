using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Justificaciones
{
    public class mdl_ObtenerArchivos
    {
        public int id { get; set; }
        public string? nombre_archivo { get; set; }
        public string? tipo_archivo { get; set; }
        public string? ruta_servidor { get; set; }
        public string? tamanio_bytes { get; set; }
        public string? fecha_carga { get; set; }
    }
}
