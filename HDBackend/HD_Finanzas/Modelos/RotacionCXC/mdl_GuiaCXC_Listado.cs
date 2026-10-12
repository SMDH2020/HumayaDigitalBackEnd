using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.RotacionCXC
{
    public class mdl_GuiaCXC_Listado
    {
        public int idlinea { get; set; }
        public string? linea { get; set; }
        public int ejercicio { get; set; }
        public string tipo_ubi { get; set; }
        public int ubicacion { get; set; }
        public double enero { get; set; }
        public double febrero { get; set; }
        public double marzo { get; set; }
        public double abril { get; set; }
        public double mayo { get; set; }
        public double junio { get; set; }
        public double julio { get; set; }
        public double agosto { get; set; }
        public double septiembre { get; set; }
        public double octubre { get; set; }
        public double noviembre { get; set; }
        public double diciembre { get; set; }
        public double semestre1 { get; set; }
        public double semestre2 { get; set; }
        public double anual { get; set; }

    }
}
