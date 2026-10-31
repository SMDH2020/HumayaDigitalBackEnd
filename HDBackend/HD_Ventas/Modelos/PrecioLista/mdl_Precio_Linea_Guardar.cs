using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos.PrecioLista
{
    public class mdl_Precio_Linea_Guardar
    {
        public int idlinea { get; set; }
        public int ejercicio { get; set; }
        public int sucursal { get; set; }
        public int periodo { get; set; }
        public double precio { get; set; }
    }
}
