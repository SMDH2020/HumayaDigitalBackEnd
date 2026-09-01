using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Reportes
{
    public class mdl_Reporte_Cobertura_Cartera
    {
        public int idasesor { get; set; }
        public string asesor { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }

        // Totales generales
        public int clientes_asignados { get; set; }
        public int clientes_visitados_total { get; set; }

        // Pequeño Productor (14)
        public int pequeño_productor_asignados { get; set; }
        public int pequeño_productor_visitados { get; set; }

        // Mediano Productor (15)
        public int mediano_productor_asignados { get; set; }
        public int mediano_productor_visitados { get; set; }

        // Gran Productor (16)
        public int gran_productor_asignados { get; set; }
        public int gran_productor_visitados { get; set; }

        // Estratégico (17)
        public int estrategico_asignados { get; set; }
        public int estrategico_visitados { get; set; }

        // Clave (18)
        public int clave_asignados { get; set; }
        public int clave_visitados { get; set; }

        // Sin Cultivos (19)
        public int sin_cultivos_asignados { get; set; }
        public int sin_cultivos_visitados { get; set; }

        // Sin Clasificar (NULL)
        public int sin_clasificar_asignados { get; set; }
        public int sin_clasificar_visitados { get; set; }
    }
}
