using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.ResultadosSucursal
{
    public class mdl_Resultados_Sucursal_Dash
    {
        //public string concepto { get; set; }
        //public string departamento { get; set; }
        //public double total { get; set; }
        //public double portotal { get; set; }

        public int idADR {  get; set; }
        public int IdSucursal { get; set; }
        public string sucursal { get; set; }
        public float total { get; set; }
        public float porcentaje { get; set; }
    }
}
