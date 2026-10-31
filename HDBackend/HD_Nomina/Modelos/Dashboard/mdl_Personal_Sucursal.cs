using System;
using System.Collections.Generic;
using System.Text;

namespace HD_Nomina.Modelos.Dashboard
{
    public class mdl_Personal_Sucursal
    {
        public int IDEstado { get; set; }
        public string estado { get; set; }
        public int IDSucursal { get; set; }
        public string Sucursal { get; set; }
        public string Sexo { get; set; }      // 'M' o 'H'
        public int total { get; set; }
    }
}
