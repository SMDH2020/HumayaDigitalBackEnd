using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Solicitud_Impresion
{
    public class mdl_Solicitud_Cultivos_View
    {
        public string? folio { get; set; }
        public string? cultivo { get; set; }
        public double hectareas { get; set; }
        public double rendimiento { get; set; }
        public string? mescosecha { get; set; }
        public double precio { get; set; }
        public string? tiporiego { get; set; }
        public string? temporal { get; set; }
    }
}
