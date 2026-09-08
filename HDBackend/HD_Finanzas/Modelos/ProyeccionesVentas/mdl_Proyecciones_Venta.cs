using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.ProyeccionesVentas
{
    public class mdl_Proyecciones_Venta
    {
        public int index { get; set; }
        public string departamento { get; set; }
        public string concepto { get; set; }
        public short orden { get; set; }
        public double importe { get; set; }
        public double por { get; set; }
        public double proyimporte { get; set; }
        public double proypor { get; set; }
        public string indicador {  get; set; }
        public double diffimporte { get; set; }
        public double diffpor { get; set; }
        public double lastimporte { get; set; }
        public double lastpor { get; set; }
        public double lastporyimporte { get; set; }
        public double lastproypor { get; set; }
        public double lastdiffimporte { get; set; }
        public double lastdiffpor { get; set; }
        public string clase
        {
            get
            {
                switch (concepto)
                {
                    case "Ventas Netas":
                        return "sub-total";
                    case "Utilidad Bruta":
                        return "sub-total";
                    case "Utilidad de Operación":
                        return "total";
                    case "Total Otros Ingresos":
                        return "total";
                    case "Total Otros Gastos":
                        return "total";
                    case "Utilidad":
                        return "total";
                    case "Ventas Totales":
                        return "sub-total";
                    default:
                        return "";
                }
            }
        }
    }
}
