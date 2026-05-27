using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Programar_Inventario
{
    public class mdl_Finalizacion_Diferencias
    {
        public string? familia { get; set; }
        public string? sku { get; set; }
        public string? descripcion { get; set; }
        public string? posicion { get; set; }
        public double existencia { get; set; }
        public double conteo { get; set; }
        public double diferencias { get; set; }
        public string? tipo_diferencia { get; set; }
        public double importe_dif { get; set; }
        public string? comentario { get; set; }
    }
}
