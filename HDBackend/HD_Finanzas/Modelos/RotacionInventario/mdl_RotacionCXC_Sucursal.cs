using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.RotacionInventario
{
    public class mdl_RotacionCXC_Sucursal
    {
        public int idestado { get; set; }
        public string? estado { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public int saldo_inicial { get; set; }
        public int movimiento { get; set; }
        public int saldo_final { get; set; }
        public int rotacion { get; set; }
        public int guia { get; set; }
    }
}
