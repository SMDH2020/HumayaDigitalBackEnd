using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.Estado_Resultados
{
    public class mdl_Desglose_Gastos
    {
        public float gastos_departamento { get; set; }
        public float gastos_staff_comercial { get; set; }
        public float proy_gastos_departamento { get; set; }
        public float proy_gastos_staff_comercial { get; set; }
        public float gastos_departamento_anterior { get; set; }
        public float gastos_staff_comercial_anterior { get; set; }

    }
}
