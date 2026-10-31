using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos
{
    public class mdlCarga_Scorecard_porVendedor_Dash_TablaAsesor_Importes
    {
        public string? adr { get; set; }
        public string? sucursal { get; set; }
        public string? idasesor { get; set; }
        public string? asesor { get; set; }
        public double Objetivo_Autoguiados { get; set; }
        public double Real_Autoguiados { get; set; }
        public double Comp_Autoguiados { get; set; }

        public double Objetivo_Drones { get; set; }
        public double Real_Drones { get; set; }
        public double Comp_Drones { get; set; }


        public double Objetivo_Implementos { get; set; }
        public double Real_Implementos { get; set; }
        public double comp_Implementos { get; set; }


        public double Objetivo_Jardineros { get; set; }
        public double Real_Jardineros { get; set; }
        public double Comp_Jardineros { get; set; }


        public double Objetivo_PA { get; set; }
        public double Real_PA { get; set; }
        public double Comp_PA { get; set; }


        public double Objetivo_Tractores { get; set; }
        public double Real_Tractores { get; set; }
        public double Comp_Tractores { get; set; }


        public double Objetivo_TracUsa { get; set; }
        public double Real_TracUsa { get; set; }
        public double Comp_TracUsa { get; set; }


        public double Objetivo_TriUsa { get; set; }
        public double Real_TriUsa { get; set; }
        public double Comp_TriUsa { get; set; }


        public double Objetivo_Garantia { get; set; }
        public double Real_Garantia { get; set; }
        public double Comp_Garantia { get; set; }


        public double Objetivo_Mantenimiento { get; set; }
        public double Real_Mantenimiento { get; set; }
        public double Comp_Poliza { get; set; }

        public int Objetivo_Visitas { get; set; }
        public int Real_Visitas { get; set; }
        public int Comp_Visitas { get; set; }

        public int Objetivo_Refacciones { get; set; }
        public int Real_Refacciones { get; set; }
        public int Comp_Refacciones { get; set; }

    }
}
