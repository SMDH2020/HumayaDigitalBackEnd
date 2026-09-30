using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos.EmbudoVentas
{
    public class mdl_Embudo_Ventas_Excel
    {
        public string Titulo { get; set; }
        public string VerPor { get; set; }
        public List<mdl_Embudo_Ventas_Item_Excel> Datos { get; set; }
        public List<string> Columnas { get; set; }
        public List<string> Lineas { get; set; }
        public List<string> Fases { get; set; }
        public List<string> Sucursales { get; set; }
    }
}
