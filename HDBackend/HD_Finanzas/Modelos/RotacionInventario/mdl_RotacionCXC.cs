using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.RotacionInventario
{
    public class mdl_RotacionCXC
    {
        public int idlinea { get; set; }
        public string? linea { get; set; }
        public int saldo_inicial { get; set; }
        public int movimiento { get; set; }
        public int saldo_final { get; set; }
        public int rotacion { get; set; }
        public int guia { get; set; }
         
    }
}
