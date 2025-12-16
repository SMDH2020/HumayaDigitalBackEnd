using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Modelos
{
    public class mdl_HD_Notificaciones_Listado
    {
        public int iddetalle { get; set; }
        public int iddepartamento { get; set; }
        public int idencabezado { get; set; }
        public DateTime fecha_evento { get; set; }
        public string? mensaje { get; set; }
        public int idmodulo { get; set; }
        public bool estatus { get; set; }
        public string? modulo { get; set; }
        public string? ruta { get; set; }

        public string? fecha_inicio { get; set; }
	    public string? fecha_fin {  get; set; }
	    public string? hora { get; set; }

        public string? tipo_alerta { get; set; }
        public string? dia { get; set; }
        public string? empleado { get; set; }

        public int frecuencia { get; set; }
        public string? redireccion { get; set; }


    }
}
